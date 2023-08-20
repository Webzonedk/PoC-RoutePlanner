using System.Linq;
using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System.Data;
using RoutePlanner.DataSources;

namespace RoutePlanner
{
    internal class DBManagerTwo : DataService
    {
        private string _connectionString => ConnectionString;

        /// <summary>
        /// Inserts assignments into the database, loop through all the assignmentTypes, and assign the current assignmentType to the row. 
        /// </summary>
        /// <param name="assignmentTypes">The assignmentTypes generated previously, that is being passed down to the db call.</param>
        public void InsertAssignmentTypeData(List<AssignmentType> assignmentTypes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("AssignmentTypeDescription", typeof(string));
            dt.Columns.Add("DurationInSeconds", typeof(int));
            dt.Columns.Add("TimeFrameID", typeof(int));

            foreach (var thisAssignmentType in assignmentTypes)
            {
                DataRow row = dt.NewRow();
                row["Title"] = thisAssignmentType.Title;
                row["AssignmentTypeDescription"] = thisAssignmentType.AssignmentTypeDescription;
                row["DurationInSeconds"] = thisAssignmentType.DurationInSeconds;
                row["TimeFrameID"] = thisAssignmentType.TimeFrameID;
                dt.Rows.Add(row);
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
                        bulkCopy.ColumnMappings.Add("AssignmentTypeDescription", "AssignmentTypeDescription");
                        bulkCopy.ColumnMappings.Add("DurationInSeconds", "DurationInSeconds");
                        bulkCopy.ColumnMappings.Add("TimeFrameID", "TimeFrameID");

                        bulkCopy.DestinationTableName = "AssignmentType";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{assignmentTypes.Count} row(s) inserted.");
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
        /// Inserts the generated citizens into the citizen table in the database.
        /// </summary>
        /// <param name="citizens">The citizens previously generated in the Program.cs file.</param>
        public void InsertCitizenData(List<Citizen> citizens)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CitizenName", typeof(string));
            dt.Columns.Add("ResidenceID", typeof(int));

            foreach (var thisCitizen in citizens)
            {
                DataRow row = dt.NewRow();
                row["CitizenName"] = thisCitizen.CitizenName;
                row["ResidenceID"] = thisCitizen.ResidenceID;
                dt.Rows.Add(row);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Add column mappings (assuming the database column name is also "Title")
                        bulkCopy.ColumnMappings.Add("CitizenName", "CitizenName");
                        bulkCopy.ColumnMappings.Add("ResidenceID", "ResidenceID");

                        bulkCopy.DestinationTableName = "Citizen";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{citizens.Count} row(s) inserted.");
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
        /// Inserts the TimeFrames generated into the TimeFrame table, in the database.
        /// </summary>
        /// <param name="timeframes">The timeframes previously generated in the Program.cs file.</param>
        public void InsertTimeFrameData(List<TimeFrame> timeframes)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TimeFrameStart", typeof(string));
            dt.Columns.Add("TimeFrameEnd", typeof(string));

            foreach (var timeframe in timeframes)
            {
                DataRow row = dt.NewRow();
                row["TimeFrameStart"] = timeframe.TimeFrameStart;
                row["TimeFrameEnd"] = timeframe.TimeFrameEnd;
                dt.Rows.Add(row);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Add column mappings (assuming the database column name is also "Title")
                        bulkCopy.ColumnMappings.Add("TimeFrameStart", "TimeFrameStart");
                        bulkCopy.ColumnMappings.Add("TimeFrameEnd", "TimeFrameEnd");

                        bulkCopy.DestinationTableName = "TimeFrame";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{timeframes.Count} row(s) inserted.");
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

        public List<AssignmentType> SelectAllAssignmentTypeFromDatabase()
        {
            List<AssignmentType> assignmentTypes = new List<AssignmentType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM AssignmentType WHERE AssignmentType.ID > 3;", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AssignmentType assignmentType = new AssignmentType
                                {
                                    ID = reader.GetInt32(0),
                                    Title = reader.GetString(1), // Assuming title column is at index 1
                                    AssignmentTypeDescription = reader.GetString(2), // Assuming AssignmentTypeDescription column is at index 2
                                    DurationInSeconds = reader.GetInt32(3), // Assuming DurationInSeconds column is at index 3
                                    TimeFrameID = reader.GetInt32(4), // Assuming TimeFrameID column is at index 4
                                };
                                assignmentTypes.Add(assignmentType);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading AssignmentTypes: " + ex.Message);
            }
            return assignmentTypes;
        }

        /// <summary>
        /// Inserts the given parameters and objects into the the assignment table in the database.
        /// </summary>
        /// <param name="assignments"></param>
        public void InsertAssignmentData(List<Assignment> assignments)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CitizenID", typeof(int));
            dt.Columns.Add("EmployeeTypeMasterID", typeof(int));
            dt.Columns.Add("EmployeeTypeSlaveID", typeof(int));
            dt.Columns.Add("AssignmentTypeID", typeof(int));

            foreach (var assignment in assignments)
            {
                DataRow row = dt.NewRow();
                row["CitizenID"] = assignment.CitizenID;
                row["EmployeeTypeMasterID"] = assignment.EmployeeTypeMasterID;
                row["EmployeeTypeSlaveID"] = assignment.EmployeeTypeSlaveID;
                row["AssignmentTypeID"] = assignment.AssignmentTypeID;
                dt.Rows.Add(row);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Add column mappings (assuming the database column name is also "Title")
                        bulkCopy.ColumnMappings.Add("CitizenID", "CitizenID");
                        bulkCopy.ColumnMappings.Add("EmployeeTypeMasterID", "EmployeeTypeMasterID");
                        bulkCopy.ColumnMappings.Add("EmployeeTypeSlaveID", "EmployeeTypeSlaveID");
                        bulkCopy.ColumnMappings.Add("AssignmentTypeID", "AssignmentTypeID");

                        bulkCopy.DestinationTableName = "Assignment";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{assignments.Count} row(s) inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }// Connection gets automatically closed here due to 'using' statement
        }

        public List<TimeFrame> SelectAllTimeFramesFromDatabase()
        {
            List<TimeFrame> TimeFrames = new List<TimeFrame>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM TimeFrame", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TimeFrame timeFrame = new TimeFrame
                                {
                                    Id = reader.GetInt32(0),
                                    TimeFrameStart = reader.GetDateTime(1), // Assuming TimeFrameStart column is at index 1
                                    TimeFrameEnd = reader.GetDateTime(2), // Assuming TimeFrameEnd column is at index 2
                                };
                                TimeFrames.Add(timeFrame);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading TimeFrames: " + ex.Message);
            }
            return TimeFrames;
        }

        public List<EmployeeType> SelectAllEmployeeTypesFromDatabase()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM EmployeeType", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeType employeeType = new EmployeeType
                                {
                                    ID = reader.GetInt32(0),
                                    Title = reader.GetString(1), // Assuming Title column is at index 1
                                };
                                employeeTypes.Add(employeeType);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading EmployeeTypes: " + ex.Message);
            }
            return employeeTypes;
        }

        public List<Citizen> GetAllCitizensFromDatabase()
        {
            List<Citizen> citizens = new List<Citizen>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Citizen", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Citizen citizen = new Citizen
                                {
                                    Id = reader.GetInt32(0),
                                    CitizenName = reader.GetString(1), // Assuming CitizenName column is at index 1
                                    ResidenceID = reader.GetInt32(2), // Assuming ResidenceID column is at index 1
                                };
                                citizens.Add(citizen);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading EmployeeTypes: " + ex.Message);
            }
            return citizens;
        }
    }
}
