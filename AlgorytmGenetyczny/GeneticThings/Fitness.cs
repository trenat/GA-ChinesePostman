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

        private Dictionary<(ushort, ushort), ushort[]> _map;
        private int _count;

        public Fitness(Dictionary<(ushort, ushort), ushort[]> map, bool doubled)
        {
            this._map = map;
            if (doubled)
                _count = map.Count * 4;
            else
                _count = map.Count * 2;
        }

        public double Evaluate(IChromosome chromosome)
        {
            return  (_count / (GetLength(chromosome)));
        }


        public double GetLength(IChromosome chromosome)
        {
            //if (chromosome is PermutationChromosome chr)
            //{
            //    ushort[] path = ((PermutationChromosome)chromosome).Value;
            //    var length = 0;
            //    ushort i = (ushort)path.Length;
            //    for (ushort j = 0; j < i - 1; j++)
            //    {
            //        length += path[j] > path[j + 1] ? _map[(j, (ushort)(j + 1))][0] : _map[(j, (ushort)(j + 1))][1];
            //    }
            //    if (i > _count)
            //        return length;
            //}
            if (chromosome is Chromosome chr)
            {
               var length = chr.Path.Sum(x => x.cost);
                return length;
            }

            return int.MaxValue;
        }
    }
}
