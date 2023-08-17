using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Assignment
    {
        public int Id { get; set; }
        public DayOfWeek DayOfAssignment { get; set; }
        public DateTime TimeFrameStart { get; set; }
        public DateTime TimeFrameEnd { get; set; }
        public int? AssignmentTypeID { get; set; }
    }
}