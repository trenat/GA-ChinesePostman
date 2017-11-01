using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class Pair<T1, T2, T3>
    {
        public T1 ToSourceCost { get; set; }
        public T2 ToTargetCost { get; set; }
        public T3 Third { get; set; }
    }

}
