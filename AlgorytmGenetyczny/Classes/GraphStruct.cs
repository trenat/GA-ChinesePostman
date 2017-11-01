using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class GraphStruct
    {
        public int Source { set; get; }
        public int Target { set; get; }
        public int CostToTarget { set; get; }
        public int CostToSource { set; get; }

        public GraphStruct(int source, int target, int costToTarget, int costToSource)
        {
            Source = source;
            Target = target;
            CostToSource = costToSource;
            CostToTarget = costToTarget;
        }

    }
}
