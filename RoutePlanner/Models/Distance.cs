using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Distance
    {
        public int Id { get; set; }
        public int AddressOneId { get; set; }
        public int AddressTwoId { get; set; }
        public float Duration { get; set; }
        public float DistanceInMeters { get; set; }
    }
}
