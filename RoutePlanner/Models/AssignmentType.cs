using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class is used to store the assignment type data.
    /// </summary>
    internal class AssignmentType
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string AssignmentTypeDescription { get; set; }
        public int DurationInSeconds { get; set; }
        public int TimeFrameID { get; set; }
    }
}
