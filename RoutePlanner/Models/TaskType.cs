using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class TaskType
    {
        public string Title { get; set; }
        public string TaskTypeDescription { get; set; }
        public int DurationInSeconds { get; set; }
    }
}
