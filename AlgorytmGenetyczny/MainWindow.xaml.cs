using AlgorytmGenetyczny.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Accord;
using Accord.Genetic;
using static System.Math;
using AlgorytmGenetyczny.GeneticThings;

namespace AlgorytmGenetyczny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        static Random r = new Random();
        private static RankSelection rank = new RankSelection();
        private static RouletteWheelSelection roulette = new RouletteWheelSelection();
        private static EliteSelection elite = new EliteSelection();
        private Dictionary<string, ISelectionMethod> _selectionMethods = new Dictionary<string, ISelectionMethod>()
        {
            ["Ranking"] = rank,
            ["Elitaryzm"] = elite,
            ["Ruletka"] = roulette
        };

        public int CitiesCount { set; get; } = 50;
        public int ConnectionsCount { set; get; } = 50;
        public int MaxCost { set; get; } = 10;
        public int MinCost { set; get; } = 1;
        public int MaxCostDifferent { set; get; } = 3;

        public int PopulationCount { set; get; } = 100;
        public int GenerationCount { set; get; } = 100;
        public Dictionary<string, ISelectionMethod> SelectionMethods
        {
            get
            {
                return _selectionMethods;
            }
        }
        public KeyValuePair<string, ISelectionMethod> SelectedType { set; get; }
        public double CrossoverRate { set; get; } = 0.75;
        public double MutationRate { set; get; } = 0.15;

        public bool Migration { set; get; } = false;
        public int IslandsCount { set; get; } = 10;
        public int MigrationCount { set; get; } = 10;
        public int MigrationTime { set; get; } = 10;

        public Dictionary<ushort, City> Cities { set; get; }
        public ObservableCollection<GraphStruct> Data { set; get; } = new ObservableCollection<GraphStruct>();
        public Dictionary<(ushort, ushort), ushort[]> map = null;  //source,target - costS, costT, count 
        public Object three; //w sumie miasta są już zmapowane, drzewo to Cities<> 

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedType = SelectionMethods.First();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO:: Check if RoutsCount is not less then CitiesCount 

            ConnectCities();
            int leftConnections = ConnectionsCount - (CitiesCount - 1);
            GenerateRandomConnections(leftConnections);
            MakeMap();
        }


        private void SearchRoute(object sender, RoutedEventArgs e)
        {
            Fitness fitnessFunc = new Fitness(map, false);
            Population population = new Population(PopulationCount,
                                                   new Chromosome(ConnectionsCount, Cities, r),
                                                   fitnessFunc,
                                                  (ISelectionMethod)SelectedType.Value);
            population.CrossoverRate = CrossoverRate;
            population.MutationRate = MutationRate;
            int i = 0;
            bool needToStop = false;
            while (!needToStop)
            {

                population.RunEpoch();
                // display current path
                //             ushort[] bestValue = ((ShortArrayChromosome)population.BestChromosome).Value;
                if (i > GenerationCount)
                    break;
                i++;
            }
            //population.Migrate()
        }

        private void RandCost(out int source, out int target)
        {
            source = r.Next(MinCost, MaxCost + 1);
            target = r.Next(-MaxCostDifferent, MaxCostDifferent + 1) + source;
            target = Max(target, MinCost);
            target = Min(target, MaxCost);

        }
        private void ConnectCities()
        {
            int costToSource;
            int costToTarget;
            Cities = new Dictionary<int, City>();
            City city1 = null;
            City city2 = null;

            for (int cityN = 0; cityN < CitiesCount - 1; cityN++)  // Connect all cieties
            {
                RandCost(out costToSource, out costToTarget);
                if (cityN != 0)
                {
                    city2 = new City() { Name = cityN + 1 };
                    Cities.Add(cityN + 1, city2);
                    Cities[cityN].CreateRoute(city2, costToSource, costToTarget);
                }
                else
                {
                    city1 = new City() { Name = cityN };
                    city2 = new City() { Name = cityN + 1 };
                    Cities.Add(cityN, city1);
                    Cities.Add(cityN + 1, city2);
                    city1.CreateRoute(city2, costToSource, costToTarget);
                }
            }
        }
        private void GenerateRandomConnections(int leftConnections)
        {
            int _source;
            int _target;
            int _tempTarget;
            int _tempSource;

            int costToSource;
            int costToTarget;
            for (int i = 0; i < leftConnections; i++)   // make 'leftConnections' number of connection
            {
                _source = r.Next(CitiesCount);   // rand source 
                while (true)
                {
                    _target = r.Next(CitiesCount);  //rand target, and check if is != that source
                    if (_target != _source)
                        break;
                }

                _tempTarget = _target;
                _tempSource = _source;
                RandCost(out costToSource, out costToTarget);  //ran cost from and to target

                while (true)
                {
                    if (_target != _source && Cities[_source].CreateRoute(Cities[_target], 10, 10))   //check if still target!= source, then if source doesn't already have connection to target
                    {
                        break;
                    }
                    else
                    {
                        _target++;  //else get next city as target,
                        if (_target >= CitiesCount) //ofc, next city is city#0, get 0
                            _target = 0;
                        if (_target == _tempTarget) //now check if i didn't tried it N times already ( if source is already connected to each city)
                        {
                            _source++; //then get next source city
                            if (_source >= CitiesCount)
                                _source = 0;
                            if (_source == _tempSource)
                                throw new Exception("Błąd przy dodawaniu tras, prawdopodobnie zbyt dużo połączeń");
                        }
                    }
                }
            }
        }
        private void MakeMap()
        {
            Data.Clear();
            map = new Dictionary<(ushort, ushort), ushort[]>();

            foreach (var city in Cities)
            {
                foreach (var connection in city.Value.Connections)    //Check all conections in all cities
                {
                    if (Data.Any(x => (x.Target == city.Key && x.Source == connection.Key)))  //if connection already exist, do nothing
                        continue;
                    else  // else Add date about connection ( Source, Target, CostToSource, CostToTarget) to map (used in GA) and to 'data' (used on GUI)
                    {
                        Data.Add(new GraphStruct(city.Key, connection.Key, connection.Value, Cities[connection.Key].Connections[city.Key]));
                        map[(city.Key, connection.Key)] = new ushort[]{ connection.Value,
                                                                        Cities[connection.Key].Connections[city.Key] };
                                                                                          
                }
            }
        }
    }

}
}
