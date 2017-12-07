using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class Result
    {
        public List<(ushort city, ushort cost)> BestPath { set; get; }
        public Double BestFit { set; get; }
        public Double AvargeFit { set; get; }
        public Double BestLength { set; get; }
        public int EdgesVisited { set; get; }
        public Result(List<(ushort city, ushort cost)> Path, Double BestFit, Double AvargeFit, Double Length, int EdgesVisited)
        {
            this.BestPath = Path;
            this.BestFit = BestFit;
            this.AvargeFit = AvargeFit;
            this.BestLength = Length;
            this.EdgesVisited = EdgesVisited;
        }
    }
}
