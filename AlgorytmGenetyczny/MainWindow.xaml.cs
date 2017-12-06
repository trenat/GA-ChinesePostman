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
using Microsoft.Win32;
using System.IO;

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

        public int CitiesCount { set; get; } = 5;
        public int ConnectionsCount { set; get; } = 6;
        public int MaxCost { set; get; } = 10;
        public int MinCost { set; get; } = 1;
        public int MaxCostDifferent { set; get; } = 3;

        public int PopulationCount { set; get; } = 100;
        public int GenerationCount { set; get; } = 10;
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
        private bool _isEulerPath;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedType = SelectionMethods.First();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO:: Check if RoutsCount is not less then CitiesCount 

            Cities = CreateCities((ushort)CitiesCount);
            ConnectCities();
            int leftConnections = ConnectionsCount - (CitiesCount - 1);
            GenerateRandomConnections(leftConnections);
            MakeMap();
        }

        class SelectAll : ISelectionMethod
        {
            public void ApplySelection(List<IChromosome> chromosomes, int size)
            {
                ;
            }
        }


        private void SearchRoute(object sender, RoutedEventArgs e)
        {
            
            Fitness fitnessFunc = new Fitness(map.Count, false);
            Population population = new Population(PopulationCount,
                                                   new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
                                                   fitnessFunc,
                                                  (ISelectionMethod)SelectedType.Value);
            //Population population2 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population3 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population4 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population5 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population6 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population7 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population8 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population9 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            //Population population10 = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            population.CrossoverRate = CrossoverRate;
            population.MutationRate = MutationRate;
            population.RandomSelectionPortion = .7;
            population.AutoShuffling = false;
            //population2.CrossoverRate = CrossoverRate;
            //population2.MutationRate = MutationRate;
            //population2.RandomSelectionPortion = .7;
            //population2.AutoShuffling = false;
            //population3.CrossoverRate = CrossoverRate;
            //population3.MutationRate = MutationRate;
            //population3.RandomSelectionPortion = .7;
            //population3.AutoShuffling = false;
            //population4.CrossoverRate = CrossoverRate;
            //population4.MutationRate = MutationRate;
            //population4.RandomSelectionPortion = .6;
            //population4.AutoShuffling = true;
            //population5.CrossoverRate = CrossoverRate;
            //population5.MutationRate = MutationRate;
            //population5.RandomSelectionPortion = .6;
            //population5.AutoShuffling = true;
            //population6.CrossoverRate = CrossoverRate;
            //population6.MutationRate = MutationRate;
            //population6.RandomSelectionPortion = .6;
            //population6.AutoShuffling = true;
            //population7.CrossoverRate = CrossoverRate;
            //population7.MutationRate = MutationRate;
            //population7.RandomSelectionPortion = .6;
            //population7.AutoShuffling = true;
            //population8.CrossoverRate = CrossoverRate;
            //population8.MutationRate = MutationRate;
            //population8.RandomSelectionPortion = .6;
            //population8.AutoShuffling = true;
            //population9.CrossoverRate = CrossoverRate;
            //population9.MutationRate = MutationRate;
            //population9.RandomSelectionPortion = .6;
            //population9.AutoShuffling = true;
            //population10.CrossoverRate = CrossoverRate;
            //population10.MutationRate = MutationRate;
            //population10.RandomSelectionPortion = .6;
            //population10.AutoShuffling = true;
            //Population container = new Population(PopulationCount,
            //                                       new Chromosome(Cities, r, (ushort)r.Next(CitiesCount), true, (ushort)ConnectionsCount),
            //                                       fitnessFunc,
            //                                      (ISelectionMethod)SelectedType.Value);
            int i = 0;
            bool needToStop = false;


            System.Diagnostics.Debug.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nNew run:\n");
            while (!needToStop)
            {
                //var tasks = new Task[] { Task.Run(() => {
                //    System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++"); population.RunEpoch();
                //                                                                            }),
                //                         Task.Run(() => population2.RunEpoch()),
                //                         Task.Run(() => population3.RunEpoch()),
                //                         Task.Run(() => population4.RunEpoch()),
                //                         Task.Run(() => population5.RunEpoch()),
                //                         Task.Run(() => population6.RunEpoch()),
                //                         Task.Run(() => population7.RunEpoch()),
                //                         Task.Run(() => population8.RunEpoch()),
                //                         Task.Run(() => population9.RunEpoch()),
                //                         Task.Run(() => population10.RunEpoch())
                //                                                                };
                //Task.WaitAll(tasks);

                //if (i > 1 && i % 20 == 0)
                //{
                //    try
                //    {
                //        population.Migrate(population2, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)elite);
                //        population2.Migrate(population3, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)elite);
                //        population3.Migrate(population4, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)elite);
                //        population4.Migrate(population5, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population5.Migrate(population6, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population6.Migrate(population7, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population7.Migrate(population8, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population8.Migrate(population9, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population9.Migrate(population10, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        population10.Migrate(population, (int)Math.Floor(PopulationCount / 20.0), (ISelectionMethod)SelectedType.Value);
                //        System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                //        System.Diagnostics.Debug.WriteLine("MIGRATION");
                //        System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                //    }
                //    catch
                //    {

                //        System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                //        System.Diagnostics.Debug.WriteLine("MIGRATION Failed");
                //        System.Diagnostics.Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
                //    }

                //}
                population.RunEpoch();
                //population2.RunEpoch();
                //population3.RunEpoch();


                System.Diagnostics.Debug.WriteLine("Best path: " + String.Join(",", (population.BestChromosome as Chromosome).Path) + $"\nBest fit: {population.BestChromosome.Fitness}");
                System.Diagnostics.Debug.WriteLine("Avarge fit: " + population.FitnessAvg);
                System.Diagnostics.Debug.WriteLine("Best length: " + fitnessFunc.GetLength(population.BestChromosome));
                System.Diagnostics.Debug.WriteLine("Cities visited: " + (population.BestChromosome as Chromosome).Path.Count());
                // display current path
                //             ushort[] bestValue = ((ShortArrayChromosome)population.BestChromosome).Value;
                if (i > GenerationCount)
                    break;
                i++;
            }
            //population.Migrate()
        }

        private void RandCost(out ushort source, out ushort target)
        {
            source = (ushort)r.Next(MinCost, MaxCost + 1);
            target = (ushort)(r.Next(-MaxCostDifferent, MaxCostDifferent + 1) + source);
            target = (ushort)Max(target, MinCost);
            target = (ushort)Min(target, MaxCost);

        }

        private Dictionary<ushort, City> CreateCities(ushort count)
        {
            var citiesDictionary = new Dictionary<ushort, City>();
            for (ushort i = 0; i < count; i++)
                citiesDictionary.Add(i, new City() { Name = i });
            return citiesDictionary;
        }

        private void ConnectCities()
        {
            ushort costToSource;
            ushort costToTarget;
            City sourceCity = null;
            City targetCity = null;
            for (ushort cityN = 0; cityN < CitiesCount-1; cityN++)  // Connect all cieties
            {
                RandCost(out costToSource, out costToTarget);

                sourceCity = Cities[cityN];
                targetCity = Cities[(ushort)(cityN + 1)];
                sourceCity.CreateRoute(targetCity, costToSource, costToTarget);
                
                //if (cityN != 0)
                //{
                //    city2 = new City() { Name = ((ushort)(cityN + 1)) };
                //    Cities.Add(cityN, city2);
                //    Cities[cityN].CreateRoute(city2, costToSource, costToTarget);
                //}
                //else
                //{
                //    city1 = new City() { Name = cityN };
                //    city2 = new City() { Name = ((ushort)(cityN + 1)) };
                //    Cities.Add(cityN, city1);
                //}
                //RandCost(out costToSource, out costToTarget);
                //if (cityN != 0)
                //{
                //    city2 = new City() { Name = ((ushort)(cityN + 1)) };
                //}
                //else
                //{
                //    city1 = new City() { Name = cityN };
                //    city2 = new City() { Name = ((ushort)(cityN + 1)) };
                //    Cities.Add(cityN, city1);
                //}

                //Cities.Add(((ushort)(cityN + 1)), city2);
                //city1.CreateRoute(city2, costToSource, costToTarget);
            }
        }
        private void GenerateRandomConnections(int leftConnections)
        {
            ushort _source;
            ushort _target;
            ushort _tempTarget;
            ushort _tempSource;

            ushort costToSource;
            ushort costToTarget;
            for (int i = 0; i < leftConnections; i++)   // make 'leftConnections' number of connection
            {
                _source = (ushort) r.Next(CitiesCount);   // rand source 
                while (true)
                {
                    _target =(ushort) r.Next(CitiesCount);  //rand target, and check if is != that source
                    if (_target != _source)
                        break;
                }

                _tempTarget = _target;
                _tempSource = _source;

                while (true)
                {
                    RandCost(out costToSource, out costToTarget);  //ran cost from and to target
                    if (_target != _source && Cities[_source].CreateRoute(Cities[_target], costToSource, costToTarget))   //check if still target!= source, then if source doesn't already have connection to target
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
            if(!IsConnected())
                throw new Exception("Błąd przy dodawaniu tras, prawdopodobnie zbyt dużo połączeń");
            _isEulerPath = Cities.All(x => x.Value.Connections.Count % 2 == 0);
           
        }

        HashSet<City> ConnectedCities = new HashSet<City>();

        private bool IsConnected(City c)
        {
            ConnectedCities.Add(c);
            return c.Connections.All(x => ConnectedCities.Contains(Cities[x.Key]) || IsConnected(Cities[x.Key]));
               

        }

        private bool IsConnected()
        {
            var lastCity = Cities.First().Value;
            IsConnected(lastCity);
            return ConnectedCities.Count == CitiesCount;
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
                        Data.Add(new GraphStruct(city.Key, connection.Key,
                                                          Cities[connection.Key].Connections[city.Key], connection.Value));
                        map[(city.Key, connection.Key)] = new ushort[]{
                                                          Cities[connection.Key].Connections[city.Key], connection.Value, };
                                                                                          
                }
            }
        }

            CitiesCount = Cities.Count;
            ConnectionsCount = map.Count;
        }

        private void SaveRoute(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Graph";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text |*.txt"; 
            if (dialog.ShowDialog() == true)
            {
                var stringB = new StringBuilder();
                foreach(var data in Data)
                {
                    stringB.AppendLine(data.Source + "|"
                                  + data.Target + "|"
                                  + data.CostToSource + "|"
                                  + data.CostToTarget);
                }
                File.WriteAllText(dialog.FileName, stringB.ToString());
            }

        }

        private void LoadRoute(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text |*.txt";
            if (dialog.ShowDialog() == true)
            {
                Cities.Clear();
                
                var dataLines = File.ReadAllLines(dialog.FileName);
                ushort source, target, costToSource, costToTarget;

                foreach (var data in dataLines)
                {
                   var graphData = data.Trim().Split('|').Select(x => x.Replace("|","")).ToArray();
                   if(ushort.TryParse(graphData[0], out source)
                    && ushort.TryParse(graphData[1], out target)
                    && ushort.TryParse(graphData[2], out costToSource)
                    && ushort.TryParse(graphData[3], out costToTarget))
                    {
                        if (target == source)
                            throw new Exception("błędne dane wejściowe: miasto docelowe i źrodłowe są takie same");
                        
                        if (!Cities.ContainsKey(source))
                            Cities[source] = new City() { Name = source };
                        if (!Cities.ContainsKey(target))
                            Cities[target] = new City() { Name = target };

                        Cities[source].CreateRoute(Cities[target], costToSource, costToTarget);
                    }

                }

                CitiesCount = Cities.Count;
            }
        }
    }
}
