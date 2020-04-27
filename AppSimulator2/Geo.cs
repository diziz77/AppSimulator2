using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Geo
{
    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string city { get; set; }
        public int rank { get; set; }
        public string state { get; set; }
        public List<double> coordinates { get; set; }
        public double growth_from_2000_to_2013 { get; set; }
        public int population { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class RootObject
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }

}
