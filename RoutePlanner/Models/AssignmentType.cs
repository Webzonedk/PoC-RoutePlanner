using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class AssignmentType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignmentTypeDescription { get; set; }
        public int DurationInSeconds { get; set; }
        public int TimeFrameID { get; set; }
    }
}
