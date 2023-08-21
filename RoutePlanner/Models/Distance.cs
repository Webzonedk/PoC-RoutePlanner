using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class represents a distance between two residences.
    /// How long it takes in seconds, and how many meters there are between each residence.
    /// </summary>
    internal class Distance
    {
        public int ID { get; set; }
        public int ResidenceOneID { get; set; }
        public int ResidenceTwoID { get; set; }
        public float Duration { get; set; }
        public float DistanceInMeters { get; set; }
    }
}
