using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class is used to store the day type of the employee preferences,
    /// for instance if the employee wish to work in specific days.
    /// </summary>
    internal class DayType
    {
        public int? ID { get; set; }
        public string? WorkingDayType { get; set; }
    }
}
