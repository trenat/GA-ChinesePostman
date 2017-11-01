using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class City : IEquatable<City>
    {

        public ushort Name { set; get; }
        public Dictionary<ushort, ushort> Connections { set; get; } = new Dictionary<ushort, ushort>();   //ID miasta docelowego - koszt
        private Dictionary<ushort, ushort> proxy = new Dictionary<ushort, ushort>();
        public bool CreateRoute(City target, ushort costToSource, ushort costToTarget)
        {
            if (Connections.ContainsKey(target.Name))
                return false;

            Connections[target.Name] = costToTarget;
            target.Connections[this.Name] = costToSource;
            proxy.Add((ushort)proxy.Count, target.Name);
            return true;
        }

        public ushort GetTargetNameById(ushort id)
        {
            return proxy[id]; 
        }

        public bool Equals(City other)
        {
            if (this != null && other != null)
            {
                if (ReferenceEquals(this, other))
                    return true;
                if (this.Name == other.Name)
                    return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is City city)
                return Equals(city);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Name;
        }
    }
}
