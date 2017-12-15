using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorytmGenetyczny.Classes;

namespace AlgorytmGenetyczny.GeneticThings
{
    public class Fitness : IFitnessFunction
    {

        private int _count;

        public Fitness(int count, bool doubled)
        {
            if (doubled)
                _count = count * 4;
            else
                _count = count * 2;
        }

        public double Evaluate(IChromosome chromosome)
        {
            return  (_count*10 / (GetLength(chromosome)));
        }


        public double GetLength(IChromosome chromosome)
        {
            if (chromosome is Chromosome chr)
            {
               var length = chr.Path.Sum(x => x.cost);
                return length;
            }

            return int.MaxValue;
        }
    }
}
