using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class represents a residence with address and coordinates.
    /// </summary>
    internal class Residence
    {
        public int ID { get; set; }
        public string CitizenResidence { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
