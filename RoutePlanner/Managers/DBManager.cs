using Microsoft.Data.SqlClient;
using RoutePlanner.DataSources;
using RoutePlanner.Models;
using System.Data;

namespace RoutePlanner.Managers
{
    internal class DBManager : DataService
    {


        //Region for all insert methods
        #region Insert  

        private string _connectionString => ConnectionString;

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

                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeType; DBCC CHECKIDENT ('EmployeeType', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

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
            dt.Columns.Add("WorkingDayType", typeof(string));

            foreach (var dayType in dayTypes)
            {
                dt.Rows.Add(dayType.WorkingDayType);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM DayType; DBCC CHECKIDENT ('DayType', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("WorkingDayType", "WorkingDayType");
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

                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Skill; DBCC CHECKIDENT ('Skill', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

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


                    // Clear the Citizen table first to be able to insert
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Citizen; DBCC CHECKIDENT ('Citizen', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    // Clear the Distance table first to be able to insert
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Distance; DBCC CHECKIDENT ('Distance', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Residence; DBCC CHECKIDENT ('Residence', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

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
        /// Insert data into the Distance table
        /// </summary>
        /// <param name="distances"></param>
        public void InsertDistanceData(List<Distance> distances)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ResidenceOneID", typeof(int));
            dt.Columns.Add("ResidenceTwoID", typeof(int));
            dt.Columns.Add("Duration", typeof(float));
            dt.Columns.Add("DistanceInMeters", typeof(float));

            foreach (var distance in distances)
            {
                dt.Rows.Add(distance.ResidenceOneID, distance.ResidenceTwoID, distance.Duration, distance.DistanceInMeters);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Distance; DBCC CHECKIDENT ('Distance', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("ResidenceOneID", "ResidenceOneID");
                        bulkCopy.ColumnMappings.Add("ResidenceTwoID", "ResidenceTwoID");
                        bulkCopy.ColumnMappings.Add("Duration", "Duration");
                        bulkCopy.ColumnMappings.Add("DistanceInMeters", "DistanceInMeters");
                        bulkCopy.DestinationTableName = "Distance"; // Assuming the table name is "Distance"
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{distances.Count} row(s) inserted.");
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





        public void InsertWorkingTimeSpan(List<WorkingTimeSpan> workingTimeSpans)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TimeStart", typeof(DateTime));
            dt.Columns.Add("TimeEnd", typeof(DateTime));

            foreach (var workingTimeSpan in workingTimeSpans)
            {
                // Convert TimeSpan to DateTime for database insertion
                DateTime timeStart = DateTime.Today.Add(workingTimeSpan.TimeStart);
                DateTime timeEnd = DateTime.Today.Add(workingTimeSpan.TimeEnd);

                // Handle the case where TimeEnd wraps to the next day
                if (workingTimeSpan.TimeEnd < workingTimeSpan.TimeStart)
                {
                    timeEnd = timeEnd.AddDays(1);
                }

                dt.Rows.Add(timeStart, timeEnd);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Clear the table before inserting new data
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM WorkingTimeSpan; DBCC CHECKIDENT ('WorkingTimeSpan', RESEED, 0);", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("TimeStart", "TimeStart");
                        bulkCopy.ColumnMappings.Add("TimeEnd", "TimeEnd");
                        bulkCopy.DestinationTableName = "WorkingTimeSpan";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{workingTimeSpans.Count} row(s) inserted.");
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




        public void InsertEmployees(List<Employee> employees)
        {
            // 1. Opret en DataTable der matcher Employee strukturen i databasen
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Initials", typeof(string));
            dt.Columns.Add("EmployeePassword", typeof(string));
            dt.Columns.Add("EmployeeName", typeof(string));
            dt.Columns.Add("WeeklyWorkingHours", typeof(int));
            dt.Columns.Add("EmployeeTypeID", typeof(int));

            // 2. Fyld DataTable med data fra listen af Employee objekter
            foreach (var employee in employees)
            {
                dt.Rows.Add(null, employee.Initials, employee.EmployeePassword, employee.EmployeeName, employee.WeeklyWorkingHours, employee.EmployeeTypeID);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Brug SqlBulkCopy til at indsætte data
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        bulkCopy.ColumnMappings.Add("Initials", "Initials");
                        bulkCopy.ColumnMappings.Add("EmployeePassword", "EmployeePassword");
                        bulkCopy.ColumnMappings.Add("EmployeeName", "EmployeeName");
                        bulkCopy.ColumnMappings.Add("WeeklyWorkingHours", "WeeklyWorkingHours");
                        bulkCopy.ColumnMappings.Add("EmployeeTypeID", "EmployeeTypeID");
                        bulkCopy.DestinationTableName = "Employee";
                        bulkCopy.WriteToServer(dt);
                    }

                    connection.Close();
                }

                Console.WriteLine($"\n{employees.Count} employee row(s) inserted.");
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



        #endregion







        //Region for all read methods
        #region Read    








        /// <summary>
        /// Load addresses from database - Not in use as we only need those addresses connected to Citizens - Not in use
        /// </summary>
        /// <returns>Returns List<Residence> residences </returns>
        public List<Residence> ReadAllAddressesFromDatabase()
        {
            List<Residence> residences = new List<Residence>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT ID, CitizenResidence, Latitude, Longitude FROM Residence", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Residence address = new Residence
                                {
                                    ID = reader.GetInt32(0),
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




        /// <summary>
        /// Load addresses from database - Not in use as we only need those addresses connected to Citizens
        /// </summary>
        /// <returns>Returns List<Residence> residences </returns>
        public List<Residence> ReadAddressesFromDatabaseBasedOnCitizenID(List<Citizen> citizens)
        {
            List<Residence> residences = new List<Residence>();

            // Convert the list of CitizenID to a comma-separated string
            string citizenIds = string.Join(",", citizens.Where(c => c.Id.HasValue).Select(c => c.Id.Value));

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Modify the SQL query to filter results based on CitizenID
                    string query = $"SELECT Id, citizenResidence, latitude, longitude FROM Residence WHERE Id IN ({citizenIds})";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Residence address = new Residence
                                {
                                    ID = reader.GetInt32(0),
                                    CitizenResidence = reader.GetString(1),
                                    Latitude = reader.GetString(2),
                                    Longitude = reader.GetString(3)
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
                Console.WriteLine("An error occurred while loading Citicens addresses: " + ex.Message);
            }
            return residences;
        }




        /// <summary>
        ///  Read all citizens from database
        /// </summary>
        /// <returns></returns>
        public List<Citizen> ReadCitizensFromDataBase()
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
                                    Id = reader.GetInt32(0),
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
                Console.WriteLine("An error occurred while loading citizens: " + ex.Message);
            }

            return citizens;
        }




        /// <summary>
        /// This method reads all employeeTypes from the database and returns them as a list of EmployeeType objects
        /// </summary>
        /// <returns></returns>
        public List<EmployeeType> ReadEmployeeTypesFromDataBase()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT ID, Title FROM EmployeeType", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeType employeeTypeObject = new EmployeeType
                                {
                                    ID = reader.GetInt32(0),
                                    Title = reader.GetString(1)
                                };
                                employeeTypes.Add(employeeTypeObject);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading employee types: " + ex.Message);
            }

            return employeeTypes;
        }

        #endregion


        #region Update

        #endregion




        #region Delete

        /// <summary>
        /// This method deletes all data from the database and resets the identity columns
        /// </summary>
        public void ResetDatabaseTables()
        {
            // Ordered list of tables for deletion considering constraints
            string[] tablesToDelete = {
                "EmployeeStatementPeriod", "EmployeePreference", "EmployeeEmployeeRoute", "TimeRegistration",
                "EmployeeSkill", "EmployeeRoute", "Assignment", "Employee", "Citizen", "Distance",
                "Preference", "WorkingTimespan", "DayType", "TimeFrame", "AssignmentType", "Skill",
                "Residence", "EmployeeType", "StatementPeriod"
                };

            // List of tables with identity columns to reset
            string[] tablesWithIdentity = {
                "Employee", "EmployeeType", "StatementPeriod", "TimeRegistration", "EmployeeRoute",
                "Assignment", "Citizen", "Residence", "Distance", "TimeFrame", "AssignmentType",
                "Skill", "Preference", "WorkingTimespan", "DayType", "EmployeeEmployeeRoute",
                "EmployeeSkill", "EmployeePreference", "EmployeeStatementPeriod"
                };

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    // Begin transaction
                    SqlTransaction transaction = connection.BeginTransaction();
                    command.Transaction = transaction;

                    try
                    {
                        // Delete all records from each table in the correct order
                        foreach (string table in tablesToDelete)
                        {
                            command.CommandText = $"DELETE FROM {table}";
                            command.ExecuteNonQuery();
                        }

                        // Reset identity seed for each table
                        foreach (string table in tablesWithIdentity)
                        {
                            command.CommandText = $"DBCC CHECKIDENT ('{table}', RESEED, 0)";
                            command.ExecuteNonQuery();
                        }

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction in case of an error
                        transaction.Rollback();

                        // Log or handle the exception as needed
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
                connection.Close();
                Console.WriteLine("\nAll Tables has been reset");
            }
        }
        #endregion



    }



}

