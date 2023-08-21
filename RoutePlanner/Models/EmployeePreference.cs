using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Models
{
    /// <summary>
    /// This class represents an employee preference.
    /// </summary>
    internal class EmployeePreference
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int PreferenceID { get; set; }

    }
}
