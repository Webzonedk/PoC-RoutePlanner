using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System.Data;

namespace RoutePlanner
{
    internal class DBManager
    {
        // Connection string for the database. Make sure to replace with your credentials.

        private readonly string _connectionString = "Server=LAPTOP-P6H4N3E7;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Kent

        // private readonly string _connectionString = "Server=Tinko;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Mads






        /// <summary>
        /// Method to insert data into the EmployeeType table
        /// </summary>
        /// <param name="employeeTypes"></param>
        public void InsertEmployeeTypeData(List<EmployeeType> employeeTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));

            foreach (var employeeType in employeeTypes)
            {
                dt.Rows.Add(employeeType.Title);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Add column mappings (assuming the database column name is also "Title")
                        bulkCopy.ColumnMappings.Add("Title", "Title");

                        bulkCopy.DestinationTableName = "EmployeeType";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{employeeTypes.Count} row(s) inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }



        /// <summary>
        /// Method to insert data into the DayType table
        /// </summary>
        /// <param name="dayTypes"></param>
        public void InsertDayTypeData(List<DayType> dayTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DayType", typeof(string));

            foreach (var dayType in dayTypes)
            {
                dt.Rows.Add(dayType.WorkingDayType);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("DayType", "DayType");
                        bulkCopy.DestinationTableName = "DayType";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{dayTypes.Count} row(s) inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }



        /// <summary>
        /// Method to insert data into the Skill table
        /// </summary>
        /// <param name="skills"></param>
        public void InsertSkillData(List<Skill> skills)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("SkillDescription", typeof(string));

            foreach (var skill in skills)
            {
                dt.Rows.Add(skill.Title, skill.SkillDescription);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("Title", "Title");
                        bulkCopy.ColumnMappings.Add("SkillDescription", "SkillDescription");
                        bulkCopy.DestinationTableName = "Skill";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{skills.Count} row(s) inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }



        /// <summary>
        /// Method to insert data into the Address table
        /// </summary>
        /// <param name="addresses"></param>
        public void InsertAddressData(List<ImportAddress> addresses)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CitizenResidence", typeof(string));
            dt.Columns.Add("Latitude", typeof(string));
            dt.Columns.Add("Longitude", typeof(string));

            foreach (var address in addresses)
            {
                string fullAddress = address.ToString();
                dt.Rows.Add(fullAddress, address.Wgs84koordinatBredde, address.Wgs84koordinatLængde);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("CitizenResidence", "CitizenResidence");
                        bulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                        bulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                        bulkCopy.DestinationTableName = "Residence";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{addresses.Count} row(s) inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }



        /// <summary>
        /// Load addresses from database - Not in use as we only need those addresses connected to Citizens
        /// </summary>
        /// <returns>Returns List<Residence> residences </returns>
        public List<Residence> LoadAddressesFromDatabase()
        {
            List<Residence> residences = new List<Residence>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT Id, citizenResidence, latitude, longitude FROM Residence", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Residence address = new Residence
                                {
                                    Id = reader.GetInt32(0),
                                    CitizenResidence = reader.GetString(1), // Assuming residence column is at index 1
                                    Latitude = reader.GetString(2), // Assuming latitude column is at index 2
                                    Longitude = reader.GetString(3) // Assuming longitude column is at index 3
                                };
                                residences.Add(address);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading addresses: " + ex.Message);
            }
            return residences;
        }




        public List<Citizen> GetCitizensFromDataBase()
        {
            List<Citizen> citizens = new List<Citizen>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT ID, CitizenName, ResidenceID FROM Citizen", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Citizen CitizenObject = new Citizen
                                {
                                    CitizenID = reader.GetInt32(0),
                                    CitizenName = reader.GetString(1),
                                    ResidenceID = reader.GetInt32(2)
                                };
                                citizens.Add(CitizenObject);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading addresses: " + ex.Message);
            }

            return null;
        }



        public List<Distance> AddDistancesToDataBase(List<RouteData> residences)
        {
            //List<Residence> addressesToCalculateDistanceFrom = new List<Residence>(residences);
            //List<Residence> addressesToCalculateDistanceTo = new List<Residence>(residences);
            return null;
        }





        public void InsertDistanceData(List<Distance> distances)
        {
            //foreach (Distance distance in distances)
            //{
            //    if (distances.Contains(distance))
            //    {
            //        DataTable dt = new DataTable();
            //        dt.Columns.Add("AddressOneId", typeof(int));
            //        dt.Columns.Add("AddressTwoId", typeof(int));
            //        dt.Columns.Add("Duration", typeof(float));
            //        dt.Columns.Add("DistanceInMeters", typeof(float));
            //    }
            //}
            //bulkCopy.WriteToServer(dt);
            //connection.Close();
        }
    }



}

