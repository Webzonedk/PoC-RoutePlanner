using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Citizen
    {
        public int ? CitizenID { get; set; }
        public string? CitizenName { get; set; }
        public int? ResidenceID { get; set; }
    }
}