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
                "6 = Insert Citizens \n " +
                "7 = Calculate and insert Distances \n " +
                "8 = Insert Skills \n " +
                "9 = Insert WorkingTimeSpan \n " +
                "0 = Insert Employees \n " +
                "a = Insert Skills \n " +
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
                            //Insert AssignmentTypes into db Needs to be adjusted to SOSU skills
                            dbManagerTwo.InsertAssignmentTypeData(CreateAssignmentType());
                            Console.WriteLine("AssignmentTypes inserted");
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
            /// This method is used to create a list of assignment types
            /// </summary>
            /// <returns>returns a list of type AssignmentType</returns>
            List<AssignmentType> CreateAssignmentType()
            {
                return new List<AssignmentType>()
                {
                new AssignmentType(){Title = "Alm. rengøring", DurationInSeconds = 300, AssignmentTypeDescription = "Regulær rengøring"},
                new AssignmentType(){Title = "Medicinering", DurationInSeconds = 300, AssignmentTypeDescription = "Administrering af medicin"},
                new AssignmentType(){Title = "Natklar", DurationInSeconds = 600, AssignmentTypeDescription = "Gør klar til natten"},
                new AssignmentType(){Title = "Sengelægning", DurationInSeconds = 600, AssignmentTypeDescription = "Læg i seng"},
                new AssignmentType(){Title = "Opvækning", DurationInSeconds = 1200, AssignmentTypeDescription = "Tag op af sengen"},
                new AssignmentType(){Title = "Mad", DurationInSeconds = 900, AssignmentTypeDescription = "Opvarmning af mad, samt servering"},
                };
            }

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
            };
        }
    }
}
