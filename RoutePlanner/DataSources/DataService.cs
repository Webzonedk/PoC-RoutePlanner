using Microsoft.Extensions.Configuration;
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
    /// 


    using Microsoft.Extensions.Configuration;
    using System.IO;

    internal class DataService
    {
        public string ConnectionString { get; private set; }
        public string OpenRouteServiceUrl { get; private set; }

        public DataService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<DataService>(); // Bemærk at vi bruger DataService i stedet for Program

            var configuration = builder.Build();

            ConnectionString = configuration["ConnectionStrings:ComfortCare"];
            OpenRouteServiceUrl = configuration["OpenRouteService:Url"];
        }
    }
}
