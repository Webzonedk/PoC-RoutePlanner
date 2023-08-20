using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Assignment
    {
        public int ID { get; set; }
        public int CitizenID { get; set; }
        public int TimeFrameID { get; set; }
        public int EmployeeTypeMasterID { get; set; }
        public int EmployeeTypeSlaveID { get; set; }
        public int AssignmentTypeID { get; set; }
    }
}
