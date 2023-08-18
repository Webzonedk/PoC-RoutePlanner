﻿using System;
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
                "8 = Insert TimeFrames \n " +
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

                            int assignmentsToCreate= 20;

                            for (int i = 0; i < assignmentsToCreate; i++)
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
