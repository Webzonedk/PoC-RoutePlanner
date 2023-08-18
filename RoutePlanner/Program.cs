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
using static Azure.Core.HttpHeader;

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
                "5 = Insert AssignmenType \n " +
                "6 = Calculate and insert Distances \n " +
                "7 = Insert Citizens \n " +
                "8 = Insert TimeFrames \n " +
                "9 = Insert Skills \n " +
                "0 = Insert Skills \n " +
                "a = Insert Assignments \n " +
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

                            List<string> titles = new List<string>
                            {
                                "Alm. rengøring", "Medicinadministration", "Natklar", "Sengelægning",
                                "Opvækning", "Spise- og Måltidshjælp", "Indkøb af Dagligvarer", "Transport til Aftaler", "Hjælp med Tøjvask",
                                "Aktivitetsfølgeskab", "Rengøring af Bolig", "Selskab og Samvær", "Ledsagelse til Aktiviteter", "Hjælp til Personlig Hygiejne"
                            };

                            List<int> durationInSeconds = new List<int>
                            {
                                300, 600, 1200, 3600, 1800, 2700, 2100, 4500, 1500, 2100, 2400, 2700
                            };

                            List<string> descriptions = new List<string>
                            {
                                "Regulær rengøring", "Giv medicin i henhold til dosering", "Gør klar til natten", "Læg i seng",
                                "Tag op af sengen", "Assistér med spisning og måltider", "Indkøb af nødvendige dagligvarer", "Kørsel til lægeaftaler og andre aftaler", "Hjælp til vask og foldning af tøj",
                                "Følgeskab til sociale aktiviteter", "Rengøring af bolig og fællesområder", "Tilbringe tid med borgeren i hyggeligt samvær", "Ledsagelse til fritidsaktiviteter og arrangementer", "Assistér med personlig hygiejne og bad"
                            };

                            Random random = new Random();
                            var assignmentTypes = new List<AssignmentType>();

                            //Amount of rows to create
                            int rowsToCreate = 10;

                            for (int i = 0; i < rowsToCreate; i++)
                            {
                                string title = titles[random.Next(titles.Count)];
                                int titleIndex = titles.IndexOf(title);

                                string description = descriptions[titleIndex];

                                //Assigns the data, to 
                                assignmentTypes.Add(new AssignmentType()
                                {
                                    Title = title,
                                    DurationInSeconds = random.Next(durationInSeconds.Count),
                                    AssignmentTypeDescription = description,
                                });
                            }

                            dbManagerTwo.InsertAssignmentTypeData(assignmentTypes);
                            Console.WriteLine("AssignmentType inserted");
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

                            List<Residence> residences = dbManager.ReadAllAddressesFromDatabase();

                            Random random = new Random();
                            var citizens = new List<Citizen>();

                            //Amount of rows to create
                            int rowsToCreate = 20;

                            for (int i = 0; i < rowsToCreate; i++)
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
                            List<DateTime> timeFramesStart = new List<DateTime>
                            {
                                DateTime.ParseExact("07:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("10:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("11:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("13:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("15:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("19:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("21:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("23:00:00", "HH:mm:ss", null)
                            };

                            List<DateTime> timeFramesEnd = new List<DateTime>
                            {
                                DateTime.ParseExact("10:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("11:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("13:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("15:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("19:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("21:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("23:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("07:00:00", "HH:mm:ss", null)
                            };

                            Random random = new Random();
                            var timeFrames = new List<TimeFrame>();

                            //Amount of rows to create
                            int rowsToCreate = 20;

                            for (int i = 0; i < rowsToCreate; i++)
                            {
                                DateTime timeFrameStart = timeFramesStart[random.Next(timeFramesStart.Count)];
                                int timeFrameStartIndex = timeFramesStart.IndexOf(timeFrameStart);

                                DateTime timeFrameEnd = timeFramesEnd[timeFrameStartIndex];

                                //Assigns the data, to 
                                timeFrames.Add(new TimeFrame()
                                {
                                    TimeFrameStart = timeFrameStart,
                                    TimeFrameEnd = timeFrameEnd,
                                });
                            }

                            dbManagerTwo.InsertTimeFrameData(timeFrames);
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
                    case 'a':
                        {
                            Random random = new Random();

                            //List of assignments
                            List<Assignment> assignments = new List<Assignment>();

                            //List of all citizens
                            List<Citizen> citizens = dbManagerTwo.GetAllCitizensFromDatabase();

                            //List of all timeframes possible from parameters.
                            List<TimeFrame> timeFrames = dbManagerTwo.SelectAllTimeFramesFromDatabase();

                            //List of all EmployeeTypes available from parameters.
                            List<EmployeeType> employeeTypes = dbManagerTwo.SelectAllEmployeeTypesFromDatabase();

                            //List of all assignmentTypes possible from parameters.
                            List<AssignmentType> assignmentTypes = dbManagerTwo.SelectAllAssignmentTypeFromDatabase();

                            List<Skill> skillList = new List<Skill>(); //Null for now since we're adding it later if we have time.

                            //Amount of rows to create
                            int rowsToCreate = 80;

                            for (int i = 0; i < rowsToCreate; i++)
                            {

                                int randomCitizenIndex = random.Next(citizens.Count);
                                int randomTimeFrameIndex = random.Next(timeFrames.Count);
                                int randomEmployeeTypeMasterIndex = random.Next(employeeTypes.Count);
                                int randomAssignmentTypeIndex = random.Next(assignmentTypes.Count);

                                //Assigns the data, to the Assignment model, and adds them to the "assignments" list.
                                assignments.Add(new Assignment()
                                {
                                    CitizenID = citizens[randomCitizenIndex].Id.Value,
                                    TimeFrameID = timeFrames[randomTimeFrameIndex].Id,
                                    EmployeeTypeMasterID = employeeTypes[randomEmployeeTypeMasterIndex].Id,
                                    AssignmentTypeID = assignmentTypes[randomAssignmentTypeIndex].Id,
                                });
                            }

                            dbManagerTwo.InsertAssignmentData(assignments);
                            Console.WriteLine("Assignments inserted");
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
