using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner
{

    public class RouteData
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public List<List<double>> Coordinates { get; set; }
        public string Profile { get; set; }
        public string Preference { get; set; }

        public RouteData(double distance, double duration, List<List<double>> coordinates, string profile, string preference)
        {
            Distance = distance;
            Duration = duration;
            Coordinates = coordinates;
            Profile = profile;
            Preference = preference;
        }
    }
}
