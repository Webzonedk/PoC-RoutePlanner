using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Preference
    {
        public int ID { get; set; }
        public int WorkingTimespanID { get; set; }
        public int DayTypeID { get; set; }
    }
}
