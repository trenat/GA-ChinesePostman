using Accord.Genetic;
using AlgorytmGenetyczny.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.GeneticThings
{
    public class Chromosome : ChromosomeBase
    {
        private Dictionary<ushort, City> _cities;
        private Random r;
        private ushort _basePoint;
        public HashSet<ushort> Path { private set; get; }
        private Dictionary<City, HashSet<ushort>> favoriteRoutes;  //Chromosome, for each city have it's route ranking, 
        public Chromosome(int length, Dictionary<ushort, City> cities, Random r, ushort basePoint)
        {
            _cities = cities;
            _basePoint = basePoint;
            Path = GeneratePath();
            this.r = r;
        }

        private HashSet<ushort> GeneratePath()
        {
            var tempPath = new HashSet<ushort>();
            var usedRoutesDict = new Dictionary<ushort, IList<ushort>>();

            int i = 0;
            ushort previousCityID = _basePoint;
            City previousCity = _cities[previousCityID];
            var routsCount = previousCity.Connections.Count;  // how much routes is possible from this City
            var nextRoute = (ushort)r.Next(routsCount);  //random any route 
            var target = previousCity.GetTargetNameById(nextRoute);
            tempPath.Add(GetRouteID(previousCityID, target));   //dodać ewentualnie jeśli będzie potrzebne przy liczeniu fitness
            //if (usedRoutesDict.ContainsKey(previousCityID))
            //{
            //    usedRoutesDict[previousCityID].RemoveAt(next - 1);
            //    favoriteRoutes[previousCity].Add(rand.)
            //}
            //else
            //{
            usedRoutesDict[previousCityID] = new List<ushort>((IEnumerable<ushort>)Enumerable.Range(0, (int)routsCount).Where(x => x != nextRoute)); //Generate list with possible connections excluding next route
            favoriteRoutes[previousCity] = new HashSet<ushort>() { nextRoute }; //Add nextRoute as most prefered for baseCity for this chromosome
                                                                                //}

            while (true)
            {
                previousCityID = previousCity.GetTargetNameById((ushort)(nextRoute - 1)); 
                previousCity = _cities[previousCityID];
                nextRoute = (ushort)r.Next(usedRoutesDict[previousCityID].Count);
                target = previousCity.GetTargetNameById(nextRoute);

                if () /// check if usedRoutesDict[previousCityID].Count > 0 (czy mogę gdzieś jeszcze pójść? Co jeśli nie? T_T (TEORIA Grafu eulerofskiego) 

                    //...
                    //...
                    //...

                    if (usedRoutesDict.ContainsKey(previousCityID))
                    {
                        usedRoutesDict[previousCityID].RemoveAt(nextRoute);
                        favoriteRoutes[previousCity].Add(nextRoute);
                    }
                    else
                    {
                        usedRoutesDict[previousCityID] = new List<ushort>((IEnumerable<ushort>)Enumerable.Range(0, (int)routsCount).Where(x => x != nextRoute)); //Generate list with possible connections excluding next route
                        favoriteRoutes[previousCity] = new HashSet<ushort>() { nextRoute }; //Add nextRoute as most prefered for baseCity for this chromosome
                    }
                //}

            }
            return tempPath;
        }

        protected Chromosome(ChromosomeBase source)
        {

        }

        public override void Crossover(IChromosome pair)
        {

        }

        public override void Mutate()
        {

        }

        public override IChromosome Clone()
        {
            return new Chromosome(this);
        }

        public override IChromosome CreateNew()
        {

        }

        public override void Generate()
        {

        }
    }
}
