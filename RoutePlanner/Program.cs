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
                "9 = Insert WorkingTimeSpan \n " +
                "0 = Insert Skills \n " +
                "a = Insert Assignments \n " +
                "b = Insert Skills \n " +
                "c = Insert Skills \n " +
                "d = Insert Skills \n " +
                "e = Insert Skills \n " +
                "x = exit \n " +
                "y = Delete all tables (not active) \n " +
                "z = Insert all tables (not active) \n " +
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
                            //Insert AssignmentType into db Needs to be adjusted to SOSU skills

                            List<string> titles = new List<string>
                            {
                                "Medicinadministration", "Natklar", "Sengelægning",
                                "Opvækning", "Spise- og Måltidshjælp", "Indkøb af Dagligvarer", "Transport til Aftaler", "Hjælp med Tøjvask",
                                "Aktivitetsfølgeskab", "Rengøring af Bolig", "Selskab og Samvær", "Ledsagelse til Aktiviteter", "Hjælp til Personlig Hygiejne",
                                "Natobservation", "Aftensmad", "Middagsmad"
                            };

                            List<int> durationInSeconds = new List<int>
                            {
                                300, 600, 1200, 3600, 1800, 2700, 2100, 4500, 1500, 2100, 2400, 2700
                            };

                            List<string> descriptions = new List<string>
                            {
                                "Giv medicin i henhold til dosering", "Gør klar til natten", "Læg i seng",
                                "Tag op af sengen", "Assistér med spisning og måltider", "Indkøb af nødvendige dagligvarer", "Kørsel til lægeaftaler og andre aftaler", "Hjælp til vask og foldning af tøj",
                                "Følgeskab til sociale aktiviteter", "Rengøring af bolig og fællesområder", "Tilbringe tid med borgeren i hyggeligt samvær", "Ledsagelse til fritidsaktiviteter og arrangementer", "Assistér med personlig hygiejne og bad",
                                "Observation af borgeren over hen over natten", "Aftensmads opvarmning og hjælp med at spise", "Eventuel opvarmning af middagsmad og hjælp med at spise hvis nødvendig, eller smørning af madder."
                            };

                            List<string> morningAssignments = new List<string>();
                            List<string> forenoonAssignments = new List<string>();
                            List<string> middayAssignments = new List<string>();
                            List<string> aftermiddayAssignments = new List<string>();
                            List<string> dinnerAssignments = new List<string>();
                            List<string> eveningAssignments = new List<string>();
                            List<string> bedtimeAssignments = new List<string>();
                            List<string> nightAssignments = new List<string>();

                            for (int i = 0; i < titles.Count; i++)
                            {
                                string title = titles[i].ToLower();

                                if (title.Contains("opvækning") || title.Contains("hjælp til personlig hygiejne"))
                                    morningAssignments.Add(titles[i]);
                                else if (title.Contains("spise- og måltidshjælp"))
                                    forenoonAssignments.Add(titles[i]);
                                else if (title.Contains("aktivitetsfølgeskab") || title.Contains("ledsagelse til aktiviteter") || title.Contains("rengøring af bolig") || title.Contains("indkøb af dagligvarer") || title.Contains("medicinadministration") || title.Contains("hjælp med tøjvask") || title.Contains("transport til aftaler"))
                                    middayAssignments.Add(titles[i]);
                                else if (title.Contains("medicinadministration") || title.Contains("hjælp med tøjvask") || title.Contains("transport til aftaler") || title.Contains("selskab og samvær"))
                                    aftermiddayAssignments.Add(titles[i]);
                                else if (title.Contains("middagsmad"))
                                    dinnerAssignments.Add(titles[i]);
                                else if (title.Contains("aftensmad"))
                                    eveningAssignments.Add(titles[i]);
                                else if (title.Contains("natklar") || title.Contains("sengelægning"))
                                    bedtimeAssignments.Add(titles[i]);
                                else if (title.Contains("natobservation"))
                                    nightAssignments.Add(titles[i]);

                            }


                            Random random = new Random();
                            var assignmentTypes = new List<AssignmentType>();

                            //Amount of rows to create
                            int rowsToCreate = 14;

                            for (int i = 0; i < rowsToCreate; i++)
                            {
                                string title = titles[i];
                                //int titleIndex = titles.IndexOf(title);

                                string description = descriptions[i];

                                int timeFrameID = 0;
                                if (morningAssignments.Contains(title))
                                {
                                    timeFrameID = 1;
                                }
                                else if (forenoonAssignments.Contains(title))
                                {
                                    timeFrameID = 2;
                                }
                                else if (middayAssignments.Contains(title))
                                {
                                    timeFrameID = 3;
                                }
                                else if (aftermiddayAssignments.Contains(title))
                                {
                                    timeFrameID = 4;
                                }
                                else if (dinnerAssignments.Contains(title))
                                {
                                    timeFrameID = 5;
                                }                                
                                else if (eveningAssignments.Contains(title))
                                {
                                    timeFrameID = 6;
                                }
                                else if (bedtimeAssignments.Contains(title))
                                {
                                    timeFrameID = 7;
                                }                                
                                else if (nightAssignments.Contains(title))
                                {
                                    timeFrameID = 8;
                                }

                                //if (!assignmentTypes.Any(at => at.Title == title && at.TimeFrameID == timeFrameID))
                                //{
                                assignmentTypes.Add(new AssignmentType()
                                    {
                                        Title = title,
                                        DurationInSeconds = durationInSeconds[random.Next(durationInSeconds.Count)],
                                        AssignmentTypeDescription = description,
                                        TimeFrameID = timeFrameID,
                                    });
                                //}
                            }

                            dbManagerTwo.InsertAssignmentTypeData(assignmentTypes);
                            Console.WriteLine("AssignmentType inserted");
                            break;
                        }
                    case '6':
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
                    case '7':
                        {
                            //Read citizens from db, calculate distances, and insert Distances into db
                            List<Citizen> citizens = dbManager.ReadCitizensFromDataBase();
                            List<Residence> tempAddresses = dbManager.ReadAddressesFromDatabaseBasedOnCitizenID(citizens);

                            CalculateDistancesManager calculateDistancesManager = new CalculateDistancesManager(); //Maybe an interface should be used to lower bindings
                            var distances = await calculateDistancesManager.GetDistancesAsync(tempAddresses);
                            dbManager.InsertDistanceData(distances);
                            break;
                        }
                    case '8':
                        {
                            List<DateTime> timeFramesStart = new List<DateTime>
                            {
                                DateTime.ParseExact("06:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("09:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("11:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("13:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("17:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("18:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("21:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("23:00:00", "HH:mm:ss", null),
                            };

                            List<DateTime> timeFramesEnd = new List<DateTime>
                            {
                                DateTime.ParseExact("09:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("11:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("13:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("17:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("18:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("21:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("23:00:00", "HH:mm:ss", null),
                                DateTime.ParseExact("06:00:00", "HH:mm:ss", null),
                            };

                            Random random = new Random();
                            var timeFrames = new List<TimeFrame>();

                            //Amount of rows to create
                            int rowsToCreate = 8;

                            for (int i = 0; i < rowsToCreate; i++)
                            {
                                DateTime timeFrameStart = timeFramesStart[i];
                                DateTime timeFrameEnd = timeFramesEnd[i];

                                //Assigns the data, to 
                                timeFrames.Add(new TimeFrame()
                                {
                                    TimeFrameStart = timeFrameStart,
                                    TimeFrameEnd = timeFrameEnd,
                                });
                            }

                            dbManagerTwo.InsertTimeFrameData(timeFrames);
                            Console.WriteLine("Timeframes inserted");
                            break;

                        }
                    case '9':
                        {
                            //Create WorkingTimeSpans and Insert them into db
                            dbManager.InsertWorkingTimeSpan(GetWorkingTimeSpans());

                            break;
                        }
                    case '0':
                        {
                            break;
                        }
                    case 'a':
                        {
                            Random random = new Random();

                            List<Assignment> assignments = new List<Assignment>();
                            List<Citizen> citizens = dbManagerTwo.GetAllCitizensFromDatabase();
                            List<TimeFrame> timeFrames = dbManagerTwo.SelectAllTimeFramesFromDatabase();
                            List<EmployeeType> employeeTypes = dbManagerTwo.SelectAllEmployeeTypesFromDatabase();
                            List<AssignmentType> assignmentTypes = dbManagerTwo.SelectAllAssignmentTypeFromDatabase();
                            List<Skill> skillList = new List<Skill>();

                            int rowsToCreate = 100;

                            List<Tuple<int, int>> uniqueCombinations = new List<Tuple<int, int>>();
                            foreach (var citizen in citizens)
                            {
                                foreach (var timeFrame in timeFrames)
                                {
                                    uniqueCombinations.Add(new Tuple<int, int>(citizen.Id.Value, timeFrame.Id));
                                }
                            }

                            uniqueCombinations = uniqueCombinations.OrderBy(x => random.Next()).ToList();

                            int rowsCreated = 0;
                            for (int i = 0; i < uniqueCombinations.Count && rowsCreated < rowsToCreate; i++)
                            {
                                var uniqueCombination = uniqueCombinations[i];
                                int citizenId = uniqueCombination.Item1;
                                int randomAssignmentTypeIndex = random.Next(assignmentTypes.Count);
                                int randomEmployeeTypeMasterIndex = random.Next(employeeTypes.Count);

                                assignments.Add(new Assignment()
                                {
                                    CitizenID = citizenId,
                                    TimeFrameID = assignmentTypes[randomAssignmentTypeIndex].TimeFrameID,
                                    EmployeeTypeMasterID = employeeTypes[randomEmployeeTypeMasterIndex].Id,
                                    AssignmentTypeID = assignmentTypes[randomAssignmentTypeIndex].Id,
                                });

                                rowsCreated++;
                            }

                            if (rowsCreated < rowsToCreate)
                            {
                                Console.WriteLine("Could not generate desired assignments with unique combinations.");
                            }


                            dbManagerTwo.InsertAssignmentData(assignments);
                            Console.WriteLine("Assignments inserted");
                            break;
                        }
                    case 'b':
                        {
                            break;
                        }
                    case 'c':
                        {
                            break;
                        }
                    case 'd':
                        {
                            break;
                        }
                    case 'e':
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

            /// <summary>
            /// This method is used to get the working time spans for a day
            /// </summary>
            /// <returns>returns a list og WorkingTimeSpan</returns>
            List<WorkingTimeSpan> GetWorkingTimeSpans()
            {
                return new List<WorkingTimeSpan>
                {
                new WorkingTimeSpan {TimeStart = new TimeSpan(7, 0, 0), TimeEnd = new TimeSpan(15, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(15, 0, 0), TimeEnd = new TimeSpan(23, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(23, 0, 0), TimeEnd = new TimeSpan(7, 0, 0) } //wraps to the next day
                };
            }
        }
    }
}
