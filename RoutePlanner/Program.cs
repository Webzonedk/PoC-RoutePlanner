using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using RoutePlanner.Models;
using RoutePlanner;
using RoutePlanner.Managers;

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
            List<Address>? addresses = new List<Address>();

            Console.WriteLine("Chose option: \n " +
                "1 = insert employeeType \n " +
                "2 = insert DayTypes \n " +
                "3 = Insert Skills \n " +
                "4 = import Address \n " +
                "5 = Insert TaskType \n " +
                "6 = Insert Skills \n " +
                "7 = Insert Citizens \n " +
                "8 = Insert Skills \n " +
                "9 = Insert Skills \n " +
                "0 = get route \n " +
                "");
            var run = true;
            while (run)
            {
                char input = Console.ReadKey().KeyChar;
                var dbManager = new DBManager();
                var dbManagerTwo = new DBManagerTwo();
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
                            addresses = CsvReader.LoadAddressesFromCsv();
                            dbManager.InsertAddressData(addresses);
                            Console.WriteLine("Addresses imported");
                            break;
                        }
                    case '5':
                        {
                            //Insert TaskTypes into db Needs to be adjusted to SOSU skills
                            var assignmentTypes = new List<AssignmentType>()
                        {
                            new AssignmentType(){Title = "Alm. rengøring", DurationInSeconds = 300, AssignmentTypeDescription = "Regulær rengøring"},
                            new AssignmentType(){Title = "Medicinering", DurationInSeconds = 300, AssignmentTypeDescription = "Administrering af medicin"},
                            new AssignmentType(){Title = "Natklar", DurationInSeconds = 600, AssignmentTypeDescription = "Gør klar til natten"},
                            new AssignmentType(){Title = "Sengelægning", DurationInSeconds = 600, AssignmentTypeDescription = "Læg i seng"},
                            new AssignmentType(){Title = "Opvækning", DurationInSeconds = 1200, AssignmentTypeDescription = "Tag op af sengen"},
                            new AssignmentType(){Title = "Mad", DurationInSeconds = 900, AssignmentTypeDescription = "Opvarmning af mad, samt servering"},
                        };

                            dbManagerTwo.InsertAssignmentTypeData(assignmentTypes);
                            Console.WriteLine("TaskTypes inserted");
                            break;
                        }
                    case '6':
                        {
                            break;
                        }
                    case '7':
                        {
                            List<string> names = new List<string>
                            {
                                "Ole", "Kurt", "Henning", "Torben", "Carsten",
                                "Erik", "Jens", "Lars", "Sven", "Per",
                                "Finn", "Jørgen", "Børge", "Anders", "Mogens",
                                "Knud", "Ebbe", "Allan", "Poul", "Kaj", 
                                "Bente", "Grete", "Yrsula", "Elsebet", "Ninna",
                                "Karen", "Annie", "Inger", "Lis", "Mette",
                                "Tove", "Gitte", "Ruth", "Eva", "Hanne",
                                "Birgit", "Lene", "Sofie", "Ida", "Anna"
                            };

                            Random random = new Random();
                            var citizens = new List<Citizen>();

                            int citizensToCreate = 20;

                            for (int i = 0; i < citizensToCreate; i++)
                            {
                                string name = names[random.Next(names.Count)];

                                int residenceID = random.Next(1, 3001);

                                citizens.Add(new Citizen() { CitizenName = name, ResidenceID = residenceID });
                            }

                            dbManagerTwo.InsertCitizenData(citizens);
                            Console.WriteLine("Citizens inserted");
                            break;
                        }
                    case '8':
                        {
                            //Insert Assignments into db
                            var assignments = new List<Assignment>()
                        {
                            new Assignment(){DayOfAssignment = "2023-06-15T08:00:00 6/15/2023", TimeFrameStart = "2023-06-15T08:00:00 6/15/2023", TimeFrameEnd = "2023-06-15T08:05:00 6/15/2023", AssignmentTypeID = 1},
                            new Assignment(){DayOfAssignment = "2023-08-16T08:00:00 8/16/2023", TimeFrameStart = "2023-08-16T16:00:00 8/16/2023", TimeFrameEnd = "2023-08-16T16:05:00 8/16/2023", AssignmentTypeID = 2},
                            new Assignment(){DayOfAssignment = "2023-08-14T08:00:00 8/16/2033", TimeFrameStart = "2023-08-14:25:00 8/16/2033", TimeFrameEnd = "2023-08-14:35:00 8/16/2033", AssignmentTypeID = 3},
                            new Assignment(){DayOfAssignment = "2023-06-15T08:00:00 6/15/2023", TimeFrameStart = "2023-06-15T08:25:00 6/15/2023", TimeFrameEnd = "2023-06-15T08:35:00 6/15/2023", AssignmentTypeID = 4},
                            new Assignment(){DayOfAssignment = "2023-06-15T08:00:00 6/15/2023", TimeFrameStart = "2023-06-15T12:00:00 6/15/2023", TimeFrameEnd = "2023-06-15T12:20:00 6/15/2023", AssignmentTypeID = 5},
                            new Assignment(){DayOfAssignment = "2023-06-17T08:00:00 6/15/2023", TimeFrameStart = "2023-06-1T08:00:00 6/15/2023", TimeFrameEnd = "2023-06-15T08:15:00 6/15/2023", AssignmentTypeID = 6},
                        };

                            dbManagerTwo.InsertAssignmentData(assignments);
                            Console.WriteLine("Assignments inserted");
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








    //records for the api response. as it is just simple data.
    public record ApiResponse(List<Route> Routes, Metadata Metadata);

    public record Route(RouteSummary Summary);

    public record RouteSummary(double Distance, double Duration);

    public record Metadata(Query Query);

    public record Query(List<List<double>> Coordinates, string Profile, string Preference);
}
