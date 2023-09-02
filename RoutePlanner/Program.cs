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
using System.Runtime.CompilerServices;

namespace RoutePlanner
{
    class Program
    {
        //Inputs to be able to adjust the amount of data to be created
        private static int _citizensToCreate = 1000;
        private static int _maxNumberOfAssignmentRowsToCreate = 5000; //If this number will maximum be 17 * citizens, even if You put in a higher number. This is to avoid double assignments to a citizen.

        private static int _numberOfEmployeesToCreate = 500;
        private static int _percentageOfEmployeesIsSosuAssistants = 35;
        private static int _percentOfEmployeesWorking40HoursAtWeek = 7;
        private static int _percentOfEmployeesWorking30HoursAtWeek = 35;
        private static int _percentOfEmployeesWorking25HoursAtWeek = 35;

        private static DBManager _dbManager = new DBManager();
        private static CalculateDistancesManager _calculateDistancesManager = new CalculateDistancesManager();
        private static EmployeeFactory _employeeCreaterService = new EmployeeFactory();

        static async Task Main()
        {
            Console.Clear();
            Console.WriteLine("Chose option: \n " +
                "1 = insert employeeType \n " +
                "2 = insert DayTypes \n " +
                "3 = Insert Skills \n " +
                "4 = import Address \n " +
                "5 = Insert TimeFrames \n " +
                "6 = Insert Citizens \n " +
                "7 = Calculate and insert Distances (Takes a long time depending on the amount of citizens. (ca. 25 minutes for 1000 citizens) \n " +
                "8 = Insert AssignmenType \n " +
                "9 = Insert WorkingTimeSpan \n " +
                "0 = Insert Employees \n " +
                "a = Insert Assignments \n " +
                "b = Insert Preferences \n " +
                "x = exit \n " +
                "y = Delete all tables \n " +
                //"q = Delete all tables, except Citizen, Residence and Distance, as they take a long time to generate. \n " +
                "z = Insert all tables \n\n ");
            //running a loop to keep the program running
            var run = true;
            while (run)
            {
                char input = Console.ReadKey().KeyChar;
                switch (input)
                {
                    case '1':
                        {
                            //Insert SOSO assisten and SOSU hjælper into db
                            _dbManager.InsertEmployeeTypeData(GenerateEmployeeTypes());
                            Console.WriteLine("EmployeeTypes inserted");
                            break;
                        }
                    case '2':
                        {
                            //Insert dayTypes into db
                            _dbManager.InsertDayTypeData(GenerateDayTypes());
                            Console.WriteLine("DayTypes inserted");
                            break;
                        }
                    case '3':
                        {
                            //Insert skills into db
                            _dbManager.InsertSkillData(CreateSkils());
                            Console.WriteLine("Skills inserted");
                            break;
                        }
                    case '4':
                        {
                            //Import addresses from csv and insert into db
                            List<ImportAddress> addresses = CsvReader.LoadAddressesFromCsv();
                            _dbManager.InsertAddressData(addresses);
                            Console.WriteLine("Addresses imported");
                            break;
                        }
                    case '5':
                        {
                            //Calls the method to generate all time frames and
                            //Sends the list of timeframes on to the sql statement that inserts the timeFrames into the timeFrame table.
                            _dbManager.InsertTimeFrameData(GenerateTimeframes());
                            Console.WriteLine("Timeframes inserted");
                            break;
                        }
                    case '6':
                        {
                            Console.WriteLine("\nHow many citizens do you wish to create?");
                            _citizensToCreate = int.Parse(Console.ReadLine());
                            //Calls the function to generate all citizens, and takes the paramter of an int of how many citizens you wish to generate 
                            //then it sends the list of citizens on to the sql statement that inserts the citizens into the citizen table.
                            _dbManager.InsertCitizenData(GenerateAllCitizens(_citizensToCreate));
                            Console.WriteLine("Citizens inserted");
                            break;
                        }
                    case '7':
                        {
                            //Read citizens from db, calculate distances, and insert Distances into db
                            _dbManager.InsertDistanceData(CalculateDistances());
                            Console.WriteLine("Distances inserted");
                            break;
                        }

                    case '8':
                        {
                            //Creates and insert AssignmentTypes into db
                            _dbManager.InsertAssignmentTypeData(GenerateAllAssignmentTypes());
                            Console.WriteLine("AssignmentType inserted");
                            break;
                        }
                    case '9':
                        {
                            //Create WorkingTimeSpans and Insert them into db
                            _dbManager.InsertWorkingTimeSpan(GetWorkingTimeSpans());
                            break;
                        }
                    case '0':
                        {
                            Console.WriteLine("\nHow many employees do you want to create?");
                            _numberOfEmployeesToCreate = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should be SOSU Assistents?");
                            _percentageOfEmployeesIsSosuAssistants = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should fulltime employees with 40 hours pr week?");
                            _percentOfEmployeesWorking40HoursAtWeek = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should parttime employees with 30 hours pr week?");
                            _percentOfEmployeesWorking30HoursAtWeek = int.Parse(Console.ReadLine());
                            Console.WriteLine("What percentage of the employees should parttime employees with 25 hours pr week?");
                            _percentOfEmployeesWorking25HoursAtWeek = int.Parse(Console.ReadLine());

                            //Read EmployeeTypes from db and send them to the EmployeeCreaterService who
                            //Create Employees and insert them into db
                            _dbManager.InsertEmployees(_employeeCreaterService.CreateEmployees(
                                _numberOfEmployeesToCreate,
                                _percentageOfEmployeesIsSosuAssistants,
                                _percentOfEmployeesWorking40HoursAtWeek,
                                _percentOfEmployeesWorking30HoursAtWeek,
                                _percentOfEmployeesWorking25HoursAtWeek,
                                _dbManager.ReadEmployeeTypesFromDataBase())
                                );
                            Console.WriteLine("Employees inserted");
                            break;
                        }
                    case 'a':
                        {
                            Console.WriteLine("Max number of Assignments to create (This Might be lower if amount of citicents is low");
                            _maxNumberOfAssignmentRowsToCreate = int.Parse(Console.ReadLine());
                            //Calls the function to generate all assignments, and requires paramter, of int of how many rows to generate (assignments to generate)
                            //Sends the list of assignments, on to the sql statement that inserts the assignments into the assignments table.
                            _dbManager.InsertAssignmentData(GenerateAllAssignments(_maxNumberOfAssignmentRowsToCreate));
                            Console.WriteLine("Assignments inserted to db");
                            break;
                        }
                    case 'b':
                        {
                            //Generating preferences and connecting those to employees, and inserting them into database
                            CreateAndInsertPreferencesIntoDB();
                            Console.WriteLine("Preferences inserted");
                            break;
                        }
                    case 'x':
                        {
                            run = false;
                            break;
                        }
                    case 'y':
                        {
                            _dbManager.ResetDatabaseTables();
                            break;
                        }
                    //case 'q':
                    //    {
                    //        _dbManager.DeleteAllDataExceptEssentials();
                    //        break;
                    //    }
                    case 'z':
                        {
                            CreateAllTableBasedVariablesInTop();
                            break;
                        }
                    default:
                        break;
                }
            }
        }




        /// <summary>
        /// This method first clear the Tables and afterwards fill the tables based on variables in the top of the program
        /// </summary>
        private static void CreateAllTableBasedVariablesInTop()
        {
            Console.WriteLine("\n\nResetting database");
            _dbManager.ResetDatabaseTables();
            Console.WriteLine("\n1: Inserting employeeType");
            _dbManager.InsertEmployeeTypeData(GenerateEmployeeTypes());
            Console.WriteLine("-----------");
            Console.WriteLine("\n2: Inserting DayTypes");
            _dbManager.InsertDayTypeData(GenerateDayTypes());
            Console.WriteLine("-----------");
            Console.WriteLine("\n3: Inserting Skills");
            _dbManager.InsertSkillData(CreateSkils());
            Console.WriteLine("-----------");
            Console.WriteLine("\n4: Inserting Address");
            List<ImportAddress> addresses = CsvReader.LoadAddressesFromCsv();
            _dbManager.InsertAddressData(addresses);
            Console.WriteLine("-----------");
            Console.WriteLine("\n5: Inserting TimeFrames");
            _dbManager.InsertTimeFrameData(GenerateTimeframes());
            Console.WriteLine("-----------");
            Console.WriteLine("\n6: Inserting Citizens");
            _dbManager.InsertCitizenData(GenerateAllCitizens(_citizensToCreate));
            Console.WriteLine("-----------");
            Console.WriteLine("\n7: Calculate and insert Distances");
            _dbManager.InsertDistanceData(CalculateDistances());
            Console.WriteLine("-----------");
            Console.WriteLine("\n8: Inserting AssignmenType");
            _dbManager.InsertAssignmentTypeData(GenerateAllAssignmentTypes());
            Console.WriteLine("-----------");
            Console.WriteLine("\n9: Inserting WorkingTimeSpan");
            _dbManager.InsertWorkingTimeSpan(GetWorkingTimeSpans());
            Console.WriteLine("-----------");
            Console.WriteLine("\n0: Inserting Employees");
            _dbManager.InsertEmployees(_employeeCreaterService.CreateEmployees(_numberOfEmployeesToCreate, _percentageOfEmployeesIsSosuAssistants, _percentOfEmployeesWorking40HoursAtWeek, _percentOfEmployeesWorking30HoursAtWeek, _percentOfEmployeesWorking25HoursAtWeek, _dbManager.ReadEmployeeTypesFromDataBase()));
            Console.WriteLine("-----------");
            Console.WriteLine("\na: Inserting Assignments");
            _dbManager.InsertAssignmentData(GenerateAllAssignments(_maxNumberOfAssignmentRowsToCreate));
            Console.WriteLine("-----------");
            Console.WriteLine("\nb: Inserting Preferences");
            CreateAndInsertPreferencesIntoDB();
            Console.WriteLine("-----------");
            Console.WriteLine("\nAll done... Thank You for your patience!");
        }



        /// <summary>
        /// This method generates all preferences and inserts them into the database
        /// the method also have far to many responsibilities. but that is how I got it created.
        /// </summary>
        private static void CreateAndInsertPreferencesIntoDB()
        {
            var dayTypes = _dbManager.ReadAllDayTypesFromDatabase();
            var workingTimeSpans = _dbManager.ReadAllWorkingTimeSpansFromDatabase();
            _dbManager.InsertPreferenceData(GeneratePreferences(dayTypes, workingTimeSpans));
            var preferences = _dbManager.ReadAllPreferencesFromDatabase();//Reading the inserted preferences from db to ensure that only accesible preferences are used
            var employees = _dbManager.ReadAllEmployeesFromDatabase();
            _dbManager.InsertEmployeePreferenceData(GenerateEmployeePreferencesWithUserInput(dayTypes, workingTimeSpans, preferences, employees));
        }



        /// <summary>
        /// This method is calculating the distance between all addresses in the database
        /// </summary>
        /// <returns>Returns a list of type Distance</returns>
        private static List<Distance> CalculateDistances()
        {
            try
            {
                Console.WriteLine("Starting Calculate Distances...This will take around 20 seconds for 100 citicens, but around 25 minutes for 1000 citicenz");

                // Wait for the asynchronous method to complete and get the result
                var distances = _calculateDistancesManager.GetDistancesAsync(
                    _dbManager.ReadAddressesFromDatabaseBasedOnCitizenID(
                        _dbManager.ReadCitizensFromDataBase()
                    )
                ).GetAwaiter().GetResult();

                Console.WriteLine("CalculateDistances completed successfully.");
                return distances;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in CalculateDistances: {ex.Message}");
                return new List<Distance>(); // return an empty list or null as appropriate
            }
        }




        /// <summary>
        /// This method is used to create a list of EmployeeType
        /// </summary>
        /// <returns>Returns a list of type EmployeeType</returns>
        private static List<EmployeeType> GenerateEmployeeTypes()
        {
            // List of employee titles
            var employeeTitles = new List<string>
        {
            "SOSU assistent",
            "SOSU hjælper"
        };

            // Using LINQ to create EmployeeType objects
            return employeeTitles.ConvertAll(title => new EmployeeType { Title = title });
        }




        /// <summary>
        /// This method is used to create a list of DayTypes
        /// </summary>
        /// <returns>Returns a list of type DayType</returns>
        public static List<DayType> GenerateDayTypes()
        {
            // List of working day types
            var workingDayTypes = new List<string>
        {
            "Hverdage",
            "Lørdage",
            "Søn- og helligdage"
        };

            // Using LINQ to create DayType objects
            return workingDayTypes.ConvertAll(dayType => new DayType { WorkingDayType = dayType });
        }




        /// <summary>
        /// This method is used to create a list of skills needed to accomplish speciel tasks
        /// </summary>
        /// <returns>returns a list of type Skill</returns>
        private static List<Skill> CreateSkils()
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
        }




        /// <summary>
        /// This method is used to create a list of WorkingTimeSpan
        /// </summary>
        /// <returns>Returns a list of WorkingTimeSpan</returns>
        private static List<WorkingTimeSpan> GetWorkingTimeSpans()
        {
            return new List<WorkingTimeSpan>
                {
                new WorkingTimeSpan {TimeStart = new TimeSpan(7, 0, 0), TimeEnd = new TimeSpan(15, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(15, 0, 0), TimeEnd = new TimeSpan(23, 0, 0) },
                new WorkingTimeSpan {TimeStart = new TimeSpan(23, 0, 0), TimeEnd = new TimeSpan(7, 0, 0) } //wraps to the next day
                };
        }




        /// <summary>
        /// This method is used to generate a list of AssignmentTypes
        /// </summary>
        /// <param name="rowsToCreateVAlue"></param>
        /// <returns>Returns a list of AssignmentType</returns>
        private static List<AssignmentType> GenerateAllAssignmentTypes()
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

            //TODO: Add lunchbreak
            //for (int i = 9; i < 12; i++)
            //    assignmentTypes.Add(new AssignmentType()
            //    {
            //        Title = "Medarbejder spisepasuse",
            //        DurationInSeconds = 1800,
            //        AssignmentTypeDescription = "Medarbejder køre til centralen til spisepause",
            //        TimeFrameID = i,
            //    });


            //TODO: Add rute starting time
            //for (int i = 12; i < 15; i++)
            //    assignmentTypes.Add(new AssignmentType()
            //    {
            //        Title = "Rute opstarte på Centralen",
            //        DurationInSeconds = 600,
            //        AssignmentTypeDescription = "Medarbejder møder ind, og gør klar til at køre ud på ruten",
            //        TimeFrameID = i,
            //    });

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




        /// <summary>
        /// This method generates a list of citizens, based on the amount of rows we wish to create.
        /// </summary>
        /// <param name="rowsToCreateValue"></param>
        /// <returns>Returns a list of Citizen</returns>
        private static List<Citizen> GenerateAllCitizensold(int rowsToCreateValue)
        {
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
            List<Residence> residences = _dbManager.ReadAllAddressesFromDatabase();

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



        private static List<Citizen> GenerateAllCitizens(int rowsToCreateValue)
        {
            // Get all residences from the database.
            List<Residence> residences = _dbManager.ReadAllAddressesFromDatabase();
            List<string> _firstNames = new List<string>
            {
                "Aage", "Agnes", "Alfred", "Anders", "Anna", "Arne", "Astrid", "Axel",
                "Bent", "Bente", "Birgit", "Birte", "Bjarne", "Bodil", "Britta", "Børge",
                "Carla", "Carl", "Christian", "Clara", "Dagmar", "Dorthe", "Egon", "Ejner",
                "Ella", "Ellen", "Erik", "Erna", "Ester", "Eva", "Finn", "Frede", "Frederik", "Frida",
                "Gerda", "Grete", "Grethe", "Gunnar", "Gurli", "Hans", "Harald", "Helle", "Helge",
                "Henning", "Henry", "Holger", "Inga", "Inge", "Inger", "Irene", "Ivan", "Jens",
                "Jette", "Johanne", "John", "Jørgen", "Karen", "Karl", "Kirsten", "Kjeld", "Knud",
                "Kristian", "Lars", "Laura", "Lene", "Lilly", "Lis", "Lise", "Lotte", "Mads",
                "Margit", "Margrethe", "Maria", "Marianne", "Marie", "Mette", "Michael", "Mogens",
                "Morten", "Niels", "Nina", "Ole", "Otto", "Palle", "Paul", "Peder", "Per",
                "Pernille", "Peter", "Poul", "Preben", "Rasmus", "Rigmor", "Ruth", "Sigrid",
                "Sofie", "Solveig", "Søren", "Steen", "Svend", "Thorkild", "Tove", "Ulla",
                "Valdemar", "Vera", "Verner", "Viggo", "Villy", "Wilhelm", "Yvonne"
            };

            List<string> _lastNames = new List<string>
            {
                "Andersen", "Berg", "Christensen", "Davidsen", "Eriksen", "Frandsen", "Gundersen", "Hansen",
                "Ibsen", "Jensen", "Klausen", "Larsen", "Madsen", "Nielsen", "Olsen", "Pedersen", "Petersen",
                "Rasmussen", "Sørensen", "Thomsen", "Ulrichsen", "Vestergaard", "Winther", "Aagaard", "Bisgaard",
                "Carlsen", "Dahl", "Eskildsen", "Frederiksen", "Graversen", "Hermansen", "Ingemann", "Jacobsen",
                "Kjær", "Laursen", "Mortensen", "Nedergaard", "Odgaard", "Poulsen", "Quist", "Rohde", "Skov",
                "Toft", "Uhrenholt", "Vad", "Wagner", "Aastrup", "Bruun", "Clemmensen", "Dinesen", "Ellegaard",
                "Foged", "Gregersen", "Hedegaard", "Isaksen", "Jeppesen", "Kofoed", "Lynge", "Mogensen", "Nørregaard",
                "Overgaard", "Præst", "Richter", "Sloth", "Torp", "Udsen", "Voss", "Westergaard", "Aaberg", "Bundgaard",
                "Caspersen", "Dreier", "Elmquist", "Falk", "Gravesen", "Hvid", "Iversen", "Johannesen", "Kragh",
                "Lynggaard", "Møller", "Nørup", "Odum", "Pihl", "Riis", "Steffensen", "Trier", "Ulriksen", "Vraa",
                "Wulff", "Aakjær", "Bundvad", "Clausen", "Drost", "Elmer", "Fallesen", "Gravgaard", "Hvidtfeldt",
                "Iwertz", "Jokumsen", "Kok", "Lyngsø", "Mogens", "Nørager", "Olesen", "Pind", "Rix", "Sonne",
                "Thyssen", "Uldall", "Vrist", "Würtz"
            };

            Random random = new Random();
            // List of citizens.
            var citizens = new List<Citizen>
            {
                // Add the HealthCare central as a citizen.
                new Citizen() { CitizenName = "Central", ResidenceID = 1 }
            };

            // Loop through the desired amount of rows to create.
            for (int i = 0; i < rowsToCreateValue; i++)
            {
                // Generate a random first name and last name from the lists.
                string firstName = _firstNames[random.Next(_firstNames.Count)];
                string lastName = _lastNames[random.Next(_lastNames.Count)];

                // Combine the first name and last name to create a full name.
                string fullName = $"{firstName} {lastName}";

                // Generate a random residence ID.
                int residenceID = random.Next(residences.Count);

                // Create a new Citizen object and add it to the list.
                citizens.Add(new Citizen() { CitizenName = fullName, ResidenceID = residenceID });
            }

            // Return the list of all the citizens generated.
            return citizens;
        }





        /// <summary>
        /// This method generates a list of timeframes, based on the amount of rows we wish to create.
        /// </summary>
        /// <param name="rowsToCreateValue"></param>
        /// <returns>Returns a list of TimeFrame</returns>
        private static List<TimeFrame> GenerateTimeframes()
        {
            // Liste af tuples indeholdende start- og sluttider
            var timeFramesData = new List<(string Start, string End)>
        {
            ("06:00:00", "09:00:00"),
            ("09:00:00", "11:00:00"),
            ("11:00:00", "13:00:00"),
            ("13:00:00", "17:00:00"),
            ("17:00:00", "18:30:00"),
            ("18:30:00", "21:00:00"),
            ("21:00:00", "23:00:00"),
            ("23:00:00", "06:00:00"),
            //TODO: Add extra timeframes for luncbreaks
            //("11:30:00", "12:30:00"),
            //("19:30:00", "20:30:00"),
            //("03:30:00", "04:30:00"),
            //TODO: Add extra timeframes for beginning of shift
            //("07:00:00", "07:01:00"),
            //("15:00:00", "15:01:00"),
            //("23:00:00", "23:01:00"),
        };

            // Using LINQ to create TimeFrame objects from the tuples and returning the list of timeFrames
            return timeFramesData.Select((tf, index) => new TimeFrame
            {
                Id = index + 1,
                TimeFrameStart = DateTime.ParseExact(tf.Start, "HH:mm:ss", null),
                TimeFrameEnd = DateTime.ParseExact(tf.End, "HH:mm:ss", null)
            }).ToList();
        }




        /// <summary>
        /// This method generates all the assignments, based on the input parameter containing the amount of rows to create.
        /// </summary>
        /// <param name="maxNumberOfRowsToCreate"></param>
        /// <returns>Returns a list of Assignment</returns>
        private static List<Assignment> GenerateAllAssignments(int maxNumberOfRowsToCreate)
        {
            //Connection to second database manager.
            var dbManager = new DBManager();

            Random random = new Random();

            //List of all the previous tables, so we can get the relevant data to combine.
            List<Assignment> assignments = new List<Assignment>();
            List<Citizen> citizens = dbManager.ReadCitizensFromDataBase();
            List<TimeFrame> timeFrames = dbManager.ReadAllTimeFramesFromDatabase();
            List<EmployeeType> employeeTypes = dbManager.ReadEmployeeTypesFromDataBase();
            List<AssignmentType> assignmentTypes = dbManager.ReadAllAssignmentTypeFromDatabase();
            //Null for now, can be added if we have time.
            List<Skill> skillList = new List<Skill>();

            //Rows to create based on input parameters..
            int rowsToCreate = maxNumberOfRowsToCreate;

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

            //TODO: Add lunchBreaak to assignments
            //assignments.Add(new Assignment()
            //{
            //    CitizenID = 0,
            //    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
            //    AssignmentTypeID = 1,
            //});
            //assignments.Add(new Assignment()
            //{
            //    CitizenID = 0,
            //    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
            //    AssignmentTypeID = 2,
            //});
            //assignments.Add(new Assignment()
            //{
            //    CitizenID = 0,
            //    EmployeeTypeMasterID = employeeTypes[random.Next(employeeTypes.Count)].ID,
            //    AssignmentTypeID = 3,
            //});

            //TODO beginning of route to assignments
            //for (int i = 0; i < 6; i++)
            //{

            //}

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




        /// <summary>
        /// This method is used to preferences, witch is used to tell the system how many employees it working in different days and times
        /// </summary>
        /// <returns>Returns a list of Preference</returns>
        private static List<Preference> GeneratePreferences(List<DayType> dayTypes, List<WorkingTimeSpan> workingTimeSpans)
        {
            List<Preference> preferences = new List<Preference>();

            foreach (var dayType in dayTypes)
            {
                if (dayType.ID == null)
                {
                    continue; // Skip the DayType if its ID is null
                }

                foreach (var workingTimeSpan in workingTimeSpans)
                {
                    Preference preference = new Preference
                    {

                        WorkingTimespanID = workingTimeSpan.ID,
                        DayTypeID = dayType.ID.Value
                    };

                    preferences.Add(preference);
                }
            }

            return preferences;
        }




        /// <summary>
        /// This method is used to generate a list of EmployeePreference objects with user input
        /// </summary>
        /// <param name="dayTypes"></param>
        /// <param name="workingTimeSpans"></param>
        /// <param name="employees"></param>
        /// <returns>Returns a list of EmployeeReference</returns>
        private static List<EmployeePreference> GenerateEmployeePreferencesWithUserInput(List<DayType> dayTypes, List<WorkingTimeSpan> workingTimeSpans, List<Preference> preferences, List<Employee> employees)
        {
            Random random = new Random();
            Dictionary<int, double> workingTimeSpanPercentages = new Dictionary<int, double>();
            Dictionary<int, double> dayTypePercentages = new Dictionary<int, double>();
            List<EmployeePreference> employeePreferences = new List<EmployeePreference>();
            int employeePreferenceID = 1; // Start ID for EmployeePreference objects

            Console.WriteLine("Please enter the percentage of employees willing to work during each time span (Max 100 for each choice):");

            foreach (var workingTimeSpan in workingTimeSpans)
            {
                Console.Write($"ID {workingTimeSpan.ID} = {workingTimeSpan.TimeStart}-{workingTimeSpan.TimeEnd}: ");
                double percentage = double.Parse(Console.ReadLine()) / 100.0;
                workingTimeSpanPercentages.Add(workingTimeSpan.ID, percentage);
            }

            Console.WriteLine("\nPlease enter the percentage of employees willing to work on each type of day (Max 100 for each choice):");

            foreach (var dayType in dayTypes)
            {
                if (dayType.ID == null || dayType.WorkingDayType == null)
                {
                    continue;
                }

                Console.Write($"ID {dayType.ID} = {dayType.WorkingDayType}: ");
                double percentage = double.Parse(Console.ReadLine()) / 100.0;
                dayTypePercentages.Add(dayType.ID.Value, percentage);
            }

            // Generate EmployeePreferences
            foreach (var employee in employees)
            {
                foreach (var workingTimeSpan in workingTimeSpans)
                {
                    if (random.NextDouble() < workingTimeSpanPercentages[workingTimeSpan.ID])
                    {
                        foreach (var dayType in dayTypes)
                        {
                            if (dayType.ID == null)
                            {
                                continue;
                            }

                            if (random.NextDouble() < dayTypePercentages[dayType.ID.Value])
                            {
                                int preferenceID = GetPreferenceID(workingTimeSpan.ID, dayType.ID.Value, preferences);

                                EmployeePreference employeePreference = new EmployeePreference
                                {
                                    ID = employeePreferenceID++,
                                    EmployeeID = employee.ID,
                                    PreferenceID = preferenceID
                                };

                                employeePreferences.Add(employeePreference);
                            }
                        }
                    }
                }
            }
            return employeePreferences;
        }


        // Placeholder method to get the actual PreferenceID based on WorkingTimeSpan and DayType
        private static int GetPreferenceID(int workingTimeSpanID, int dayTypeID, List<Preference> preferences)
        {
            var preference = preferences.FirstOrDefault(p => p.WorkingTimespanID == workingTimeSpanID && p.DayTypeID == dayTypeID);
            return preference?.ID ?? 0;
        }



    }
}
