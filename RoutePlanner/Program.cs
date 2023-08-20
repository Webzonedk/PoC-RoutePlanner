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
using RoutePlanner.Services;
using static Azure.Core.HttpHeader;
using System.Linq;

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
                "6 = Insert Citizens \n " +
                "7 = Calculate and insert Distances \n " +
                "8 = Insert TimeFrames \n " +
                "9 = Insert WorkingTimeSpan \n " +
                "0 = Insert Employees \n " +
                "a = Insert Assignments \n " +
                "b = Insert Skills \n " +
                "c = Insert Skills \n " +
                "d = Insert Skills \n " +
                "e = Insert Skills \n " +
                "x = exit \n " +
                "y = Delete all tables \n " +
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
                            //Insert skills into db
                            dbManager.InsertSkillData(CreateSkils());
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
                            List<AssignmentType> assignmentTypes = GenerateAllAssignmentTypes(14);
                            dbManagerTwo.InsertAssignmentTypeData(assignmentTypes);
                            Console.WriteLine("AssignmentType inserted");
                            break;
                        }
                    case '6':
                        {
                            //Calls the function to generate all citizens, and takes the paramter of an int of howmany citizens you wish to genrate 
                            List<Citizen> citizens = GenerateAllCitizens(20);

                            //Sends the list of citizens on to the sql statement that inserts the citizens into the citizen table.
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
                            //Calls the function to generate all time frames, requires paramter of int, which is how many timeframes to create.
                            List<TimeFrame> timeFrames = GenerateTimeframes(11);

                            //Sends the list of timeframes on to the sql statement that inserts the timeFrames into the timeFrame table.
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
                            Console.WriteLine("How many employees do you want to create?");
                            int numberOfEmployees = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should be SOSU Assistents?");
                            int percentageOfFirstType = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should fulltime employees with 37 hours pr week?");
                            int hours37Percentage = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should parttime employees with 30 hours pr week?");
                            int hours30Percentage = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should parttime employees with 25 hours pr week?");
                            int hours25Percentage = int.Parse(Console.ReadLine());



                            //Read EmployeeTypes from db
                            List<EmployeeType> employeeTypes = dbManager.ReadEmployeeTypesFromDataBase();

                            //Create Employees and insert them into db
                            EmployeeCreaterService employeeCreaterService = new EmployeeCreaterService();

                            dbManager.InsertEmployees(employeeCreaterService.CreateEmployees(numberOfEmployees, percentageOfFirstType, hours37Percentage, hours30Percentage, hours25Percentage, employeeTypes));
                            Console.WriteLine("Employees inserted");
                            break;
                        }
                    case 'a':
                        {
                            //Calls the function to generate all assignments, and requires paramter, of int of how many rows to generate (assignments to generate)
                            List<Assignment> assignments = GenerateAllAssignments(100);
                            //Sends the list of assignments, on to the sql statement that inserts the assignments into the assignments table.
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
                    case 'y':
                        {
                            dbManager.ResetDatabaseTables();
                            break;
                        }
                    case 'z':
                        {
                            break;
                        }
                    default:
                        break;
                }
            }


            /// <summary>
            /// This method is used to create a list of skills needed to accomplish speciel tasks
            /// </summary>
            /// <returns>returns a list of type Skill</returns>
            List<Skill> CreateSkils()
            {
                return new List<Skill>()
                {
                new Skill { ID = 1, Title = "Medicinhåndtering", SkillDescription = "Oplæring i korrekt medicinhåndtering, herunder dosering, bivirkninger og interaktioner mellem forskellige lægemidler." },
                new Skill { ID = 2, Title = "Sårpleje", SkillDescription = "Oplæring i pleje af komplekse sår for at sikre korrekt behandling og heling." },
                new Skill { ID = 3, Title = "Brug af løfte- og forflytningsudstyr", SkillDescription = "Oplæring i korrekt brug af forskellige typer løfte- og forflytningsudstyr for at undgå skader." },
                new Skill { ID = 4, Title = "Sondeernæring", SkillDescription = "Oplæring i indføring og håndtering af sonder for at sikre korrekt ernæring." },
                new Skill { ID = 5, Title = "Håndtering af stomi", SkillDescription = "Oplæring i korrekt pleje og skift af stomiposer." },
                new Skill { ID = 6, Title = "Injektioner", SkillDescription = "Oplæring i administration af injektioner, som insulin, for at sikre korrekt dosering og teknik." },
                new Skill { ID = 7, Title = "Kolostomi- og ileostomipleje", SkillDescription = "Oplæring i pleje af disse stomier for at sikre korrekt hygiejne." },
                new Skill { ID = 8, Title = "Katheterpleje", SkillDescription = "Oplæring i både anlæggelse og pleje af kathetre for at undgå infektioner." },
                new Skill { ID = 9, Title = "Håndtering af demens", SkillDescription = "Oplæring i teknikker og metoder til at arbejde med borgere med demens." },
                new Skill { ID = 10, Title = "Håndtering af aggressive borgere", SkillDescription = "Oplæring i deeskaleringsteknikker og håndtering af aggressive eller udfordrende borgere." },
                new Skill { ID = 11, Title = "Håndtering af inkontinens", SkillDescription = "Oplæring i korrekt pleje og skift af inkontinensprodukter samt rådgivning til borgere om inkontinens." },
                new Skill { ID = 12, Title = "Palliativ pleje", SkillDescription = "Oplæring i pleje af døende borgere og deres pårørende, herunder smertelindring og psykologisk støtte." },
                new Skill { ID = 13, Title = "Ernæringsstøtte", SkillDescription = "Oplæring i at vurdere borgernes ernæringsbehov og give rådgivning om korrekt ernæring." },
                new Skill { ID = 14, Title = "Mundhygiejne", SkillDescription = "Oplæring i at yde korrekt mundpleje for at forebygge mundsygdomme og sikre god mundhygiejne." },
                new Skill { ID = 15, Title = "Håndtering af kroniske sygdomme", SkillDescription = "Oplæring i pleje og støtte til borgere med kroniske sygdomme som diabetes, KOL, hjertesygdomme osv." },
                new Skill { ID = 16, Title = "Kommunikation med demente", SkillDescription = "Specifikke teknikker og metoder til at kommunikere effektivt med borgere, der lider af demens." },
                new Skill { ID = 17, Title = "Brug af teknologiske hjælpemidler", SkillDescription = "Oplæring i brug af forskellige teknologiske hjælpemidler, der kan støtte i plejen af ældre borgere." },
                new Skill { ID = 18, Title = "Forebyggelse af fald", SkillDescription = "Oplæring i metoder og teknikker til at forebygge fald hos ældre borgere." },
                new Skill { ID = 19, Title = "Psykosocial støtte", SkillDescription = "Oplæring i at yde psykosocial støtte og rådgivning til borgere, der oplever ensomhed, angst eller depression." },
                new Skill { ID = 20, Title = "Rehabilitering", SkillDescription = "Grundlæggende kendskab til rehabiliteringsteknikker for at støtte borgere i at genoprette deres funktionsevne efter sygdom eller skade." }
                };
            };

            /// <summary>
            /// This method is used to get the working time spans for a day
            /// </summary>
            /// <returns>returns a list of type WorkingTimeSpan</returns>
            List<WorkingTimeSpan> GetWorkingTimeSpans()
            {
                return new List<WorkingTimeSpan>
                {
                new WorkingTimeSpan {TimeStart = new TimeSpan(7, 0, 0), TimeEnd = new TimeSpan(15, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(15, 0, 0), TimeEnd = new TimeSpan(23, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(23, 0, 0), TimeEnd = new TimeSpan(7, 0, 0) } //wraps to the next day
                };
            }

            List<AssignmentType> GenerateAllAssignmentTypes(int rowsToCreateVAlue)
            {

                //List of titles, ordered in same index order of Descriptions list.
                List<string> titles = new List<string>
                {
                    "Medicinadministration", "Natklar", "Sengelægning",
                    "Opvækning", "Spise- og Måltidshjælp", "Indkøb af Dagligvarer", "Transport til Aftaler", "Hjælp med Tøjvask",
                    "Aktivitetsfølgeskab", "Rengøring af Bolig", "Selskab og Samvær", "Ledsagelse til Aktiviteter", "Hjælp til Personlig Hygiejne",
                    "Natobservation", "Aftensmad", "Middagsmad"
                };

                // List of random realistic durations in seconds.
                List<int> durationInSeconds = new List<int>
                {
                    300, 600, 1200, 3600, 1800, 2700, 2100, 4500, 1500, 2100, 2400, 2700
                };

                //List of Descriptions, ordered in same index order of Titles list.
                List<string> descriptions = new List<string>
                {
                    "Giv medicin i henhold til dosering", "Gør klar til natten", "Læg i seng",
                    "Tag op af sengen", "Assistér med spisning og måltider", "Indkøb af nødvendige dagligvarer", "Kørsel til lægeaftaler og andre aftaler", "Hjælp til vask og foldning af tøj",
                    "Følgeskab til sociale aktiviteter", "Rengøring af bolig og fællesområder", "Tilbringe tid med borgeren i hyggeligt samvær", "Ledsagelse til fritidsaktiviteter og arrangementer", "Assistér med personlig hygiejne og bad",
                    "Observation af borgeren over hen over natten", "Aftensmads opvarmning og hjælp med at spise", "Eventuel opvarmning af middagsmad og hjælp med at spise hvis nødvendig, eller smørning af madder."
                };

                // A list of morningAssignments
                List<string> morningAssignments = new List<string>();
                // A list of forenoonAssignments
                List<string> forenoonAssignments = new List<string>();
                // A list of middayAssignments
                List<string> middayAssignments = new List<string>();
                // A list of aftermiddayAssignments
                List<string> aftermiddayAssignments = new List<string>();
                // A list of dinnerAssignments
                List<string> dinnerAssignments = new List<string>();
                // A list of eveningAssignments
                List<string> eveningAssignments = new List<string>();
                // A list of bedtimeAssignments
                List<string> bedtimeAssignments = new List<string>();
                // A list of nightAssignments
                List<string> nightAssignments = new List<string>();

                // Loop through all the titles we've created.
                for (int i = 0; i < titles.Count; i++)
                {
                    // Look at the index of titles of where we are in the loop and get the text and make it lower chars.
                    string title = titles[i].ToLower();

                    //Sorts through all the assignments, and adds them to their respective lists, based on when on the day the assignment needs to be executed.
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

                //Crates a random value we can use later.
                Random random = new Random();
                //Create a list of assignmentTypes.
                List<AssignmentType> assignmentTypes = new List<AssignmentType>();

                //Amount of rows to create
                int rowsToCreate = 14;

                for (int i = 9; i < 12; i++)
                assignmentTypes.Add(new AssignmentType()
                {
                    Title = "Medarbejder spisepasuse",
                    DurationInSeconds = 1800,
                    AssignmentTypeDescription = "Medarbejder køre til centralen til spisepause",
                    TimeFrameID = i,
                });

                //A loop based on the amount of rows we wish to create, in this case 14.
                for (int i = 0; i < rowsToCreate; i++)
                {
                    string title = titles[i];

                    string description = descriptions[i];

                    int timeFrameID = 0;

                    // Look at the title, and assign a timeframe id to it based on which list it's in.
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

                    //Assign the attributes gained from the above logic.
                    assignmentTypes.Add(new AssignmentType()
                    {
                        Title = title,
                        //Picks a random duration from the duration list, since it can vary from citizen to citizen.
                        DurationInSeconds = durationInSeconds[random.Next(durationInSeconds.Count)],
                        AssignmentTypeDescription = description,
                        TimeFrameID = timeFrameID,
                    });
                }
                
                //Returns a list of assignmentTypes.
                return assignmentTypes;
            }

            List<Citizen> GenerateAllCitizens(int rowsToCreateValue)
            {
                //Connection to first database manager.
                DBManager dbManager = new DBManager();

                //List of names to pick from.
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

                //Get all residences, from the database.
                List<Residence> residences = dbManager.ReadAllAddressesFromDatabase();

                Random random = new Random();
                //list of citizens.
                var citizens = new List<Citizen>();

                //Amount of rows to create
                int rowsToCreate = rowsToCreateValue;

                citizens.Add(new Citizen() { CitizenName = "Central", ResidenceID = 1 });

                //Loop through the desired amount of rows to create.
                for (int i = 0; i < rowsToCreate; i++)
                {
                    string name = names[random.Next(names.Count)];

                    int residenceID = random.Next(residences.Count);

                    citizens.Add(new Citizen() { CitizenName = name, ResidenceID = residenceID });
                }

                //Return a list of all the citizens generated.
                return citizens;
            }
            
            List<TimeFrame> GenerateTimeframes(int rowsToCreateValue)
            {
                //List of TimeFrameStart times. Ordered to match the timeFrameEnd list on index's
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
                                DateTime.ParseExact("11:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("19:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("03:30:00", "HH:mm:ss", null),
                            };

                //List of timeframeEnd times. Ordered to match the timeFrameStart list on index's
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
                                DateTime.ParseExact("12:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("20:30:00", "HH:mm:ss", null),
                                DateTime.ParseExact("04:30:00", "HH:mm:ss", null),
                            };

                Random random = new Random();
                var timeFrames = new List<TimeFrame>();

                //Amount of rows to create
                int rowsToCreate = rowsToCreateValue;

                //loops through rows to creatre.
                for (int i = 0; i < rowsToCreate; i++)
                {
                    //Uses same index to get the correct time start and end.
                    DateTime timeFrameStart = timeFramesStart[i];
                    DateTime timeFrameEnd = timeFramesEnd[i];

                    //Assigns the data, to the timeframe.
                    timeFrames.Add(new TimeFrame()
                    {
                        TimeFrameStart = timeFrameStart,
                        TimeFrameEnd = timeFrameEnd,
                    });
                }

                //Return the list of timeFrames.
                return timeFrames;
            }

            List<Assignment> GenerateAllAssignments(int rowsToCreateValue) {
                //Connection to second database manager.
                var dbManagerTwo = new DBManagerTwo();

                Random random = new Random();

                //List of all the previous tables, so we can get the relevant data to combine.
                List<Assignment> assignments = new List<Assignment>();
                List<Citizen> citizens = dbManagerTwo.GetAllCitizensFromDatabase();
                List<TimeFrame> timeFrames = dbManagerTwo.SelectAllTimeFramesFromDatabase();
                List<EmployeeType> employeeTypes = dbManagerTwo.SelectAllEmployeeTypesFromDatabase();
                List<AssignmentType> assignmentTypes = dbManagerTwo.SelectAllAssignmentTypeFromDatabase();
                //Null for now, can be added if we have time.
                List<Skill> skillList = new List<Skill>();

                //Rows to create based on input parameters..
                int rowsToCreate = rowsToCreateValue;

                //List of tuples, to contain unique combinations so we can find a limit to not give an assignment multiple of the same assignments in the test data.
                List<Tuple<int, int>> uniqueCombinations = new List<Tuple<int, int>>();
                //Loop through every citizen, 
                foreach (var citizen in citizens)
                {
                    //Loop through every timeframe to find which citizen has been used, and what timeframe they have.
                    foreach (var timeFrame in timeFrames)
                    {
                        uniqueCombinations.Add(new Tuple<int, int>(citizen.Id.Value, timeFrame.Id));
                    }
                }

                //Generates a random number, for each tuple, and sort them based on the numbers, in the unique combinations list.
                uniqueCombinations = uniqueCombinations.OrderBy(x => random.Next()).ToList();

                assignments.Add(new Assignment()
                {
                    CitizenID = 0,
                    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
                    AssignmentTypeID = 1,
                });
                assignments.Add(new Assignment()
                {
                    CitizenID = 0,
                    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
                    AssignmentTypeID = 2,
                });
                assignments.Add(new Assignment()
                {
                    CitizenID = 0,
                    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
                    AssignmentTypeID = 3,
                });

                int rowsCreated = 0;
                //Loop through all uniqueCombinations till all the rows desired has been created.
                for (int i = 0; i < uniqueCombinations.Count && rowsCreated < rowsToCreate; i++)
                {
                    //Save a value from the lists.
                    var uniqueCombination = uniqueCombinations[i];
                    int citizenId = uniqueCombination.Item1;
                    int randomAssignmentTypeIndex = random.Next(assignmentTypes.Count);
                    int randomEmployeeTypeMasterIndex = random.Next(employeeTypes.Count);

                    //Create a new assignment, and add it to the assignments list with the selected values froma bove.
                    assignments.Add(new Assignment()
                    {
                        CitizenID = citizenId,
                        EmployeeTypeMasterID = employeeTypes[randomEmployeeTypeMasterIndex].ID,
                        AssignmentTypeID = assignmentTypes[randomAssignmentTypeIndex].ID,
                    });

                    rowsCreated++;
                }

                //If the rows created cannot meet the desired amount, write this error prompt.
                if (rowsCreated < rowsToCreate)
                {
                    Console.WriteLine("Could not generate desired assignments with unique combinations.");
                }

                //Returns a list of assignments.
                return assignments;
            }
        }
    }
}
