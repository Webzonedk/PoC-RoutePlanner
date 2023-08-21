using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class is used for the deserialization of the response from the distance matrix API.
    /// </summary>
    internal class DistanceApiResponseClasses
    {
        public class ApiResponse
        {
            public List<Route> Routes { get; set; }
            public Metadata Metadata { get; set; }
        }

        public class Route
        {
            public RouteSummary Summary { get; set; }
        }

        public class RouteSummary
        {
            public double Distance { get; set; }
            public double Duration { get; set; }
        }

        public class Metadata
        {
            public Query Query { get; set; }
        }

        public class Query
        {
            public List<List<double>> Coordinates { get; set; }
            public string Profile { get; set; }
            public string Preference { get; set; }
        }
    }
}
