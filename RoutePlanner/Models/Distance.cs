using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Distance
    {
        public int ID { get; set; }
        public int ResidenceOneID { get; set; }
        public int ResidenceTwoID { get; set; }
        public float Duration { get; set; }
        public float DistanceInMeters { get; set; }
    }
}
