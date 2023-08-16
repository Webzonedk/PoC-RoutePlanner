using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System.Data;

namespace RoutePlanner
{
    internal class DBManager
    {
        // Connection string for the database. Make sure to replace with your credentials.
        private readonly string _connectionString = "Server=LAPTOP-P6H4N3E7;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Kent

        //private readonly string _connectionString = "Server=ComfortCare;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Mads



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
        public void InsertAddressData(List<Address> addresses)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Residence", typeof(string));
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
                        bulkCopy.ColumnMappings.Add("Residence", "Residence");
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


    }
}
