using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Assignment
    {
        public string? DayOfAssignment { get; set; }
        public string? TimeFrameStart { get; set; }
        public string? TimeFrameEnd { get; set; }
        public int? AssignmentTypeID { get; set; }
    }
}
