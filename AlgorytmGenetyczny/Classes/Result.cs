using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class Result
    {
        public int GenerationNumber { set; get; }
        public List<(ushort city, ushort cost)> BestPath { set; get; }
        public Double BestFit { set; get; }
        public Double AvargeFit { set; get; }
        public Double BestLength { set; get; }
        public Double EdgesVisited { set; get; }
        public Result(int genetartioNumber, List<(ushort city, ushort cost)> path, Double bestFit, Double avargeFit, Double length, double edgesVisited)
        {
            this.GenerationNumber = genetartioNumber;
            this.BestPath = path;
            this.BestFit = bestFit;
            this.AvargeFit = avargeFit;
            this.BestLength = length;
            this.EdgesVisited = edgesVisited;
        }

        public Result()
        {
        }

        public static Result operator +(Result a, Result b)
        {
            return new Result(a.GenerationNumber, null, a.BestFit + b.BestFit, a.AvargeFit + b.AvargeFit, a.BestLength + b.BestLength, a.EdgesVisited + b.EdgesVisited);
        }

        public static Result operator /(Result a, int b)
        {
            return new Result(a.GenerationNumber, null, a.BestFit / b, a.AvargeFit / b, a.BestLength / b, a.EdgesVisited / b);
        }
    }
}

