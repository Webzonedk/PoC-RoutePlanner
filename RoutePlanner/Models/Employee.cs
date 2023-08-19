using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    internal class Employee
    {
        public int ID { get; set; }
        public string Initials { get; set; }
        public string EmployeePassword { get; set; }
        public string EmployeeName { get; set; }
        public int WeeklyWorkingHours { get; set; }
        public int EmployeeTypeID { get; set; }
        public int SkillID { get; set; }

    }
}
