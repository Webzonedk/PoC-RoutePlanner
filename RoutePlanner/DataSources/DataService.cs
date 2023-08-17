using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.DataSources
{
    /// <summary>
    /// Class for connecting to the database and the openrouteservice api
    /// </summary>
    internal class DataService
    {
        // Connection string for the database. Make sure to replace with your credentials.

        internal readonly string _connectionString = "Server=LAPTOP-P6H4N3E7;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Kent

        //internal readonly string _connectionString = "Server=Tinko;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Mads



        /// <summary>
        /// Connections to the openrouteservice api
        /// </summary>

        //For use with public API, but is limited to 2000 requests per day
        //internal readonly string Url = "https://api.openrouteservice.org/v2/directions/driving-car";
        //internal readonly string AuthorizationKey = "5b3ce3597851110001cf62481e7382465ac54de19127d303893c63ba";  

        //For use with local API with no request limit
        //internal readonly string Url = "http://10.108.137.103:8080/ors/v2/directions/driving-car"; //School ip
        internal readonly string Url = "http://192.168.3.73:8080/ors/v2/directions/driving-car"; // Home ip
    }
}
