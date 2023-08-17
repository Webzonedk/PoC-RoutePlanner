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


        static async Task Main()
        {
            List<ImportAddress>? addresses = new List<ImportAddress>();

            Console.WriteLine("Chose option: \n " +
                "1 = insert employeeType \n " +
                "2 = insert DayTypes \n " +
                "3 = Insert Skills \n " +
                "4 = import Address \n " +
                "5 = Insert TaskType \n " +
                "6 = Calculate and insert Distances \n " +
                "7 = Insert Citizens \n " +
                "8 = Insert Skills \n " +
                "9 = Insert Skills \n " +
                "0 = Insert Skills \n " +
                "a = Insert Skills \n " +
                "b = Insert Skills \n " +
                "c = Insert Skills \n " +
                "d = Insert Skills \n " +
                "e = Insert Skills \n " +
                "");
            //running a loop to keep the program running
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
                            //Import addresses from csv and insert into db
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
                            //Read citizens from db, calculate distances, and insert Distances into db
                            List<Citizen> citizens = dbManager.ReadCitizensFromDataBase();
                            List<Residence> tempAddresses = dbManager.ReadAddressesFromDatabaseBasedOnCitizenID(citizens);

                            CalculateDistancesManager calculateDistancesManager = new CalculateDistancesManager(); //Maybe an interface should be used to lower bindings
                            var distances = await calculateDistancesManager.GetDistancesAsync(tempAddresses);
                            dbManager.InsertDistanceData(distances);
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

                            List<Residence> residences = dbManager.LoadAddressesFromDatabase();

                            Random random = new Random();
                            var citizens = new List<Citizen>();

                            int citizensToCreate = 20;

                            for (int i = 0; i < citizensToCreate; i++)
                            {
                                string name = names[random.Next(names.Count)];

                                int residenceID = random.Next(residences.Count);

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
    }
}
