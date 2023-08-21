using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class represents a citizen.
    /// </summary>
    internal class Citizen
    {
        public int? Id { get; set; }
        public string? CitizenName { get; set; }
        public int? ResidenceID { get; set; }
    }
}