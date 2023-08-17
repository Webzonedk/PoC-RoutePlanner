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
                "6 = Calculate Distances \n " +
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
                            var taskTypes = new List<TaskType>()
                        {
                            new TaskType(){Title = "Alm. rengøring", DurationInSeconds = 300, TaskTypeDescription = "Regulær rengøring"},
                            new TaskType(){Title = "Medicinering", DurationInSeconds = 300, TaskTypeDescription = "Administrering af medicin"},
                            new TaskType(){Title = "Bad", DurationInSeconds = 600, TaskTypeDescription = "Bad af borger."},
                        };

                            dbManagerTwo.InsertTaskTypeData(taskTypes);
                            Console.WriteLine("TaskTypes inserted");
                            break;
                        }
                    case '6':
                        {
                            List<Residence> tempAddresses = dbManager.LoadAddressesFromDatabase();

                            CalculateDistancesManager calculateDistancesManager = new CalculateDistancesManager(); //Maybe an interface should be used to lower bindings
                            var routeData = await calculateDistancesManager.GetRouteDataAsync(tempAddresses);

                            List<Distance> distances = dbManager.AddDistancesToDataBase(routeData);
                            dbManager.InsertDistanceData(distances);
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
