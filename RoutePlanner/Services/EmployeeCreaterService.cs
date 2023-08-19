using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutePlanner.Services
{
    internal class EmployeeCreaterService
    {
        private readonly List<string> _firstNames; // Antages at være forudfyldt med 100 danske fornavne
        private readonly List<string> _lastNames;  // Antages at være forudfyldt med 100 danske efternavne


        public EmployeeCreaterService()
        {
            _firstNames = new List<string>
            {
                "Anders", "Birgitte", "Christian", "Dorte", "Erik", "Freja", "Gustav", "Helle",
                "Ivan", "Johanne", "Klaus", "Lone", "Mikkel", "Nina", "Ole", "Pernille",
                "Quintus", "Rikke", "Steen", "Tina", "Ulf", "Vibeke", "William", "Xenia",
                "Yvonne", "Zacharias", "Astrid", "Benny", "Cecilie", "David", "Elisabeth", "Finn",
                "Grethe", "Henrik", "Ida", "Jakob", "Karen", "Lars", "Mette", "Niels",
                "Olivia", "Peter", "Ruth", "Søren", "Trine", "Ulrikke", "Victor", "Winnie",
                "Xander", "Yasmine", "Zeus", "Anna", "Bo", "Camilla", "Daniel", "Eva",
                "Frank", "Gitte", "Hans", "Ingrid", "Jesper", "Katrine", "Leon", "Maria",
                "Nikolaj", "Odin", "Pia", "Ronny", "Susanne", "Thomas", "Ulla", "Vincent",
                "Wilma", "Xanthe", "Yngve", "Zenia", "Anton", "Britt", "Carl", "Diana",
                "Emil", "Frida", "Georg", "Hanne", "Ingolf", "Jette", "Kurt", "Lise",
                "Martin", "Nancy", "Otto", "Paula", "Robert", "Signe", "Torben", "Ursula",
                "Verner", "Wanda", "Xerxes", "Yara", "Zorro"
            };
            _lastNames = new List<string>
            {
                "Andersen", "Berg", "Christensen", "Davidsen", "Eriksen", "Frandsen", "Gundersen", "Hansen",
                "Ibsen", "Jensen", "Klausen", "Larsen", "Madsen", "Nielsen", "Olsen", "Petersen",
                "Quist", "Rasmussen", "Sørensen", "Thomsen", "Ulrichsen", "Vestergaard", "Winther", "Xavier",
                "Yde", "Zachariassen", "Arnesen", "Bruun", "Carlsen", "Dahl", "Engberg", "Foged",
                "Graversen", "Hermansen", "Iversen", "Jacobsen", "Kjær", "Lund", "Mortensen", "Nørregaard",
                "Odgaard", "Poulsen", "Ravn", "Skov", "Toft", "Uhrenholt", "Vad", "Wagner",
                "Xander", "Yilmaz", "Zimmermann", "Asmussen", "Brix", "Clemmensen", "Dyhr", "Esbjerg",
                "Friis", "Gregersen", "Hedegaard", "Ingemann", "Johannsen", "Kofoed", "Lykke",
                "Mogensen", "Nyholm", "Overgaard", "Præst", "Rohde", "Steffensen", "Torp", "Underbjerg",
                "Villadsen", "Westergaard", "Xiao", "Younes", "Zachariasen", "Aagaard", "Bundgaard",
                "Caspersen", "Dreier", "Ellegaard", "Falk", "Greve", "Hvid", "Isaksen", "Jeppesen",
                "Kragh", "Lynge", "Møller", "Nedergaard", "Ottosen", "Pihl", "Richter", "Sloth",
                "Trier", "Udsen", "Voss", "Wulff", "Xu", "Yahya", "Zachariassen"
            };
        }


        public List<Employee> CreateEmployees(int numberOfEmployees, int percentageOfFirstType, int hours37Percentage, int hours30Percentage, int hours25Percentage, List<EmployeeType> employeeTypes)
        {
            var employees = new List<Employee>();

            // Generating employees firstnames, lastnames and initials
            for (int i = 0; i < numberOfEmployees; i++)
            {
                var firstName = _firstNames[new Random().Next(0, _firstNames.Count)];
                var lastName = _lastNames[new Random().Next(0, _lastNames.Count)];
                var initials = $"{firstName[0]}{firstName[1]}{lastName[0]}{lastName[1]}"; // Creating initials from first and last name

                // ensure that initials are unique
                int count = 1;
                while (employees.Any(e => e.Initials == initials))
                {
                    initials = $"{firstName[0]}{firstName[1]}{lastName[0]}{lastName[1]}{count}";
                    count++;
                }

                // Generating working hours
                int workingHours;
                int rand = new Random().Next(1, 101); // Generate a number between 1 and 100
                if (rand <= hours37Percentage)
                    workingHours = 37;
                else if (rand <= hours37Percentage + hours30Percentage)
                    workingHours = 30;
                else if (rand <= hours37Percentage + hours30Percentage + hours25Percentage)
                    workingHours = 25;
                else
                    workingHours = 45;

                // Generating employee type
                int employeeTypeID;
                if (i < numberOfEmployees * percentageOfFirstType / 100.0)
                    employeeTypeID = employeeTypes[0].ID;
                else
                    employeeTypeID = employeeTypes[1].ID; // Hardcoded 2 types in DB SOSU hjælper and SOSU Assistent

                // Adding employee to list
                employees.Add(new Employee
                {
                    Initials = initials,
                    EmployeePassword = "Kode1234!",
                    EmployeeName = $"{firstName} {lastName}",
                    WeeklyWorkingHours = workingHours,
                    EmployeeTypeID = employeeTypeID
                });
            }
            return employees;
        }

    }
}
