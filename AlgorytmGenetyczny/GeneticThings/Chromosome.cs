using Accord.Genetic;
using AlgorytmGenetyczny.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.GeneticThings
{
    public class Chromosome : ChromosomeBase
    {
        private static Dictionary<ushort, City> _cities;
        private static Random r;
        private static ushort _basePoint;
        private static bool _doubled;
        private static ushort _countsOfEdges;
        private static bool _crossoverMix;
        private static bool _killBothParents;

        public List<(ushort city, ushort cost)> Path { get; set; }
        private ConcurrentDictionary<City, List<ushort>> FavoriteRoutes { set; get; }  //Chromosome, for each city have it's route ranking, 
        public Chromosome(Dictionary<ushort, City> cities, Random rnd, ushort basePoint, bool doubled, ushort countsOfEdges, bool crossoverMix, bool killBothParents)
        {
            _cities = cities;
            _basePoint = basePoint;
            r = rnd;
            _doubled = doubled;
            _countsOfEdges = countsOfEdges;
            _crossoverMix = crossoverMix;
            _killBothParents = killBothParents;
            FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>();
            Path = GeneratePath();

        }

        private Dictionary<ushort, IList<ushort>> FillLeftRoutes(Dictionary<ushort, IList<ushort>> dictionary)
        {
            foreach (var city in _cities)
            {
                dictionary[city.Key] = city.Value.Connections.Select(x => x.Key).ToList();
                if (_doubled)
                    dictionary[city.Key] = dictionary[city.Key].Concat(city.Value.Connections.Select(x => x.Key).ToList()).ToList();
            }
            return dictionary;
        }

        private List<(ushort city, ushort cost)> GeneratePath(ConcurrentDictionary<City, List<ushort>> prefered = null)
        {
            var tempPath = new List<(ushort city, ushort cost)>();
            var leftRoutes = FillLeftRoutes(new Dictionary<ushort, IList<ushort>>());
            ushort? endingCity = null;
            var visitedEdges = new Dictionary<(ushort,ushort), object>();
            Stack<ushort> stack = new Stack<ushort>();
            ushort source;                                              // = _basePoint;
            City previousCity = null;                                                  // = _cities[previousCityID];
                                                                                       // (ushort)r.Next(routsCount);  //random any route 
            ushort target = _basePoint;                                         // = previousCity.GetTargetNameById(nextRoute);
            ushort tempTarget;
            ushort randRoute;

            stack.Push(_basePoint);


            while (true)
            {
                source = target;    // set as current      
                previousCity = _cities[source];
                if (leftRoutes[source].Count == 0)
                {
                    if (stack.Count != 0)
                    {
                        target = stack.Pop();
                        if ((target == _basePoint || target == endingCity) && visitedEdges.Count == _countsOfEdges)
                            break; 
                        if (target == source)
                            target = stack.Pop();
                        tempPath.Add((source, previousCity.Connections[target]));  //add to circuit
                        stack.Push(target);  //add target agai
                        endingCity = target;
                        continue;
                    }
                    else
                        break;

                }
                else  //else random new target 
                {

                    randRoute = (ushort)r.Next(leftRoutes[source].Count());
                    target = leftRoutes[source][randRoute];
                    for(int i = 0; i< 5; i++)
                    {
                        if (_doubled && leftRoutes[source].Count(x => x == target) == 1)   //prefered routes that have more than 1 connection
                        {
                            randRoute = (ushort)r.Next(leftRoutes[source].Count());
                            target = leftRoutes[source][randRoute];
                        }
                        else
                            break;

                    }

                    if (prefered != null && prefered.ContainsKey(previousCity))
                    {
                        int indexer = prefered[previousCity].Count;
                        while(prefered[previousCity].Count > 0)
                        {
                            try
                            {
                                tempTarget = prefered[previousCity].FirstOrDefault();
                                if (leftRoutes[source].Contains(tempTarget))
                                {
                                    target = tempTarget;
                                    prefered[previousCity].Remove(tempTarget);
                                    break;
                                }
                                prefered[previousCity].Remove(tempTarget);

                            }
                            catch
                            {
                                target = leftRoutes[source][randRoute];
                                break;
                            }
                        }
                    }
                }



                visitedEdges[(Math.Min(source,target),Math.Max(source,target))] = true;
                stack.Push(target);         //Add target to stack
                                            //remove from possible routes 
                leftRoutes[source].Remove(target);
                leftRoutes[target].Remove(source);

                if (FavoriteRoutes.ContainsKey(previousCity))
                    FavoriteRoutes[previousCity].Add(target);
                else
                    FavoriteRoutes[previousCity] = new List<ushort>() { target };


                if ((target == _basePoint || target == endingCity) && visitedEdges.Count == _countsOfEdges)
                    break;

            }

            ushort nextCity = tempPath.Count > 0 ? tempPath.Last().city : stack.Pop();  //Fill path
            while (stack.Count > 0)
                tempPath.Add((nextCity, _cities[nextCity].Connections[nextCity = stack.Pop()]));
            tempPath.Add((nextCity, 0)); //last city doesn't have more connecitons, so cost is 0;

            prefered = null;
            visitedEdges = null;
            leftRoutes = null;
            stack = null;

            return tempPath;
        }

        protected Chromosome(IChromosome source)
        {
            if(source is Chromosome ch)
            {
                FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>(ch.FavoriteRoutes);
                Path = new List<(ushort city, ushort cost)>(ch.Path);
            }
            else
                Path = GeneratePath();

        }

        private Chromosome()
        {
            FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>();
            Path = GeneratePath();

        }
        public override void Crossover(IChromosome pair)
        {
            if (pair is Chromosome ch)
            {
                int part1Legnth = (int)Math.Floor(FavoriteRoutes.Count / 3.0); //Take 40% of old preferences
                int part2Legnth = FavoriteRoutes.Count - part1Legnth;




                //cross genes 
                var preferedRoutesChild1 = ch.FavoriteRoutes
                                               .OrderBy(x => _crossoverMix ? 0 : x.Key.Name)
                                               .Take(part1Legnth)
                                               .Concat(this.FavoriteRoutes)//.Take(part1Legnth))
                                               .GroupBy(d => d.Key)
                                               .Select(x => new KeyValuePair<City, List<ushort>>(x.Key, (x.Last().Value.Count > x.First().Value.Count ? x.First().Value : x.Last().Value).ToList<ushort>()));
                //.ToDictionary(x => x.Key, x => x.First().Value);
                var preferedRoutesChild2 = this.FavoriteRoutes
                                               .OrderBy(x => _crossoverMix ? 0 : x.Key.Name)
                                               .Take(part1Legnth)
                                               .Concat(ch.FavoriteRoutes)//.Take(part1Legnth))
                                               .GroupBy(d => d.Key)
                                               .Select(x => new KeyValuePair<City, List<ushort>>(x.Key, (x.Last().Value.Count > x.First().Value.Count ? x.First().Value : x.Last().Value).ToList<ushort>()));
                //.ToDictionary(x => x.Key, x => x.First().Value);


                // replace parents wit children
                if (!_killBothParents)
                {
                    this.FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>(preferedRoutesChild1);
                    ch.FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>(preferedRoutesChild2);
                    this.Path = GeneratePath(this.FavoriteRoutes);
                    ch.Path = GeneratePath(ch.FavoriteRoutes);
                }
                else
                {
                    var killedParent = r.Next(0, 2) == 0 ? ch : this;
                    var choosenChildPref = r.Next(0, 2) == 0 ? preferedRoutesChild1 : preferedRoutesChild2;
                    killedParent.FavoriteRoutes = new ConcurrentDictionary<City, List<ushort>>(preferedRoutesChild1); ;
                    killedParent.Path = GeneratePath(killedParent.FavoriteRoutes);
                }
            }
        }
        public override void Mutate()
        {
            int i = r.Next((int)FavoriteRoutes.Count);
            for(; i>0;i--)
            {
                List<ushort> _tmp;
                var rnd = r.Next(this.FavoriteRoutes.Count);
                var c = (City)rnd;
                bool fff = this.FavoriteRoutes.ContainsKey(c);
                this.FavoriteRoutes.TryRemove(c, out _tmp);
            }
        }

        public override IChromosome Clone()
        {
            return new Chromosome(this);
        }

        public override IChromosome CreateNew()
        {
            var Chr = new Chromosome();
           
            return Chr;
        }

        public override void Generate()
        {

        }
    }
}
