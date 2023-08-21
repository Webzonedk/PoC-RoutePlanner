using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class represents a preference.
    /// </summary>
    internal class Preference
    {
        public int ID { get; set; }
        public int WorkingTimespanID { get; set; }
        public int DayTypeID { get; set; }
    }
}
