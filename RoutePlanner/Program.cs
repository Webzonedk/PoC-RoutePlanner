using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using RoutePlanner.Models;
using RoutePlanner;

namespace RoutePlanner
{
    class Program
    {
        //private const string Url = "https://api.openrouteservice.org/v2/directions/driving-car";
        //private const string AuthorizationKey = "5b3ce3597851110001cf62481e7382465ac54de19127d303893c63ba";  

        //private const string Url = "http://10.108.137.62:8080/ors/v2/directions/driving-car"; //School ip
        private const string Url = "http://192.168.3.73:8080/ors/v2/directions/driving-car"; // Home ip

        static async Task Main()
        {
            Console.WriteLine("Chose option: \n " +
                "1 = insert employeeType \n" +
                "2 = insert DayTypes \n" +
                "3 = Insert Skills \n " +
                "4 = Insert Skills \n " +
                "5 = Insert Skills \n " +
                "6 = Insert Skills \n " +
                "7 = Insert Skills \n " +
                "8 = Insert Skills \n " +
                "9 = Insert Skills \n " +
                "0 = get route \n " +
                "");
            var run = true;
            while (run)
            {
                char input = Console.ReadKey().KeyChar;
                var dbManager = new DBManager();
                switch (input)
                {
                    case '1':
                        {
                            //Insert SOSO assisten and SOSU hjælper into db
                            var employeeType = new List<EmployeeType>()
                        {
                            new EmployeeType(){Title = "SOSU assistent"},
                            new EmployeeType(){Title = "SOSU hjælper"}
                        };
                            dbManager.InsertEmployeeTypeData(employeeType);
                            Console.WriteLine("EmployeeTypes inserted");
                            break;
                        }
                    case '2':
                        {
                            //Insert dayTypes into db
                            var dayTypes = new List<DayType>()
                        {
                            new DayType(){WorkingDayType = "Hverdage"},
                            new DayType(){WorkingDayType = "Lørdage"},
                            new DayType(){WorkingDayType = "Søn- og helligdage"}
                        };

                            dbManager.InsertDayTypeData(dayTypes);
                            Console.WriteLine("DayTypes inserted");
                            break;
                        }
                    case '3':
                        {
                            //Insert dayTypes into db Needs to be adjusted to SOSU skills
                            var skills = new List<Skill>()
                        {
                            new Skill(){Title = "Alm. rengøring"},
                            new Skill(){Title = "Vinduespudsning"},
                            new Skill(){Title = "Tøjvask"},
                            new Skill(){Title = "Indkøb"},
                            new Skill(){Title = "Madlavning"},
                            new Skill(){Title = "Havearbejde"},
                            new Skill(){Title = "Husdyr"},
                            new Skill(){Title = "Barnepasning"},
                            new Skill(){Title = "Hjælpemidler"},
                            new Skill(){Title = "Personlig pleje"},
                            new Skill(){Title = "Medicin"},
                            new Skill(){Title = "Kørsel"},
                            new Skill(){Title = "Aktiviteter"},
                            new Skill(){Title = "Kommunikation"},
                            new Skill(){Title = "IT"},
                            new Skill(){Title = "Sprog"},
                            new Skill(){Title = "Andet"}
                        };

                            dbManager.InsertSkillData(skills);
                            Console.WriteLine("Skills inserted");
                            break;
                        }
                    case '4':
                        {
                            break;
                        }
                    case '5':
                        {
                            break;
                        }
                    case '6':
                        {
                            break;
                        }
                    case '7':
                        {
                            break;
                        }
                    case '8':
                        {
                            break;
                        }
                    case '9':
                        {
                            break;
                        }
                    case '0':
                        {
                            var routeData = await GetRouteDataAsync();
                            Debug.WriteLine($"Distance: {routeData.Distance}");
                            Debug.WriteLine($"Duration: {routeData.Duration}");
                            Debug.WriteLine($"Coordinates: {string.Join(", ", routeData.Coordinates)}");
                            Debug.WriteLine($"Profile: {routeData.Profile}");
                            Debug.WriteLine($"Preference: {routeData.Preference}");
                            break;
                        }
                    case 'x':
                        {
                            run = false;
                            break;
                        }
                    default:
                        break;
                }
            }





        }



        /// <summary>
        /// Getting the routeData from the api and deserializing it into a RouteData object.
        /// </summary>
        /// <returns>return an object with routedate of type RouteData</returns>
        /// <exception cref="Exception"></exception>
        static async Task<RouteData> GetRouteDataAsync()
        {
            using var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Authorization", AuthorizationKey);
            //gathering the string with parameters for the api.
            var content = new StringContent("{\"coordinates\":[[11.949467,55.322397],[12.118901,55.249683]],\"instructions\":\"false\",\"preference\":\"recommended\"}", Encoding.UTF8, "application/json");
            //getting the response from the api.
            var response = await httpClient.PostAsync(Url, content);
            //reading the response as a string.
            var responseBody = await response.Content.ReadAsStringAsync();
            //deserializing the response into a ApiResponse object.
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true//ensureing that mistakes does not happen if the content is upper og lowercased.
            };
            var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);
            //catching if the response is null or empty.
            if (jsonResponse?.Routes == null || !jsonResponse.Routes.Any())
            {
                throw new Exception("Routes data is missing in the API response.");
            }
            //creating a new RouteData object with the data from the api.
            var routeData = new RouteData(
                jsonResponse.Routes[0].Summary.Distance,
                jsonResponse.Routes[0].Summary.Duration,
                jsonResponse.Metadata.Query.Coordinates,
                jsonResponse.Metadata.Query.Profile,
                jsonResponse.Metadata.Query.Preference
            );

            return routeData;
        }
    }






    class Skill
    {
        public string Title { get; set; }
        public string SkillDescription { get; set; }
    }

    //records for the api response. as it is just simple data.
    public record ApiResponse(List<Route> Routes, Metadata Metadata);

    public record Route(RouteSummary Summary);

    public record RouteSummary(double Distance, double Duration);

    public record Metadata(Query Query);

    public record Query(List<List<double>> Coordinates, string Profile, string Preference);
}
