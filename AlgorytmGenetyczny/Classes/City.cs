using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmGenetyczny.Classes
{
    public class City : IEquatable<City>, IComparable<City>
    {

        public ushort Name { set; get; }
        public Dictionary<ushort, ushort> Connections { private set; get; } = new Dictionary<ushort, ushort>();   //ID miasta docelowego - koszt

        public int CompareTo(City other)
        {
            if (other.Name > Name)
                return 1;
            if (other.Name < Name)
                return 1;
            return 0;
        }

        public bool CreateRoute(City target, ushort costToSource, ushort costToTarget)
        {
            if (Connections.ContainsKey(target.Name))  // a co jesli chce zdublować połączenia??? 
                return false;

            Connections[target.Name] = costToTarget;
            target.Connections[this.Name] = costToSource;
            //proxy.Add((ushort)proxy.Count, target.Name);   //sprawdź czy może mozesz tutaj zdublować? jak są liczone 
            //target.proxy.Add((ushort)target.proxy.Count, Name);
            return true;
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

        public static explicit operator City(int v)
        {
            return new City() { Name = (ushort)v };
        }
    }
}
