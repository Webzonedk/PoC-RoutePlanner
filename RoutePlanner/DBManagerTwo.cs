using System.Linq;
using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System.Data;

namespace RoutePlanner
{
    internal class DBManagerTwo
    {
        // Connection string for the database. Make sure to replace with your credentials.
        //private readonly string _connectionString = "Server=LAPTOP-P6H4N3E7;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Kent


        private readonly string _connectionString = "Server=Tinko;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true";

        public void InsertAssignmentTypeData(List<AssignmentType> assignmentTypes)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // SQL command to insert data into the table.
                string sqlCommandText = "INSERT INTO AssignmentType (Title, AssignmentTypeDescription, DurationInSeconds) VALUES (@Title, @AssignmentTypeDescription, @DurationInSeconds)";

                foreach (AssignmentType assignmentType in assignmentTypes)
                {
                    using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@Title", assignmentType.Title);
                        command.Parameters.AddWithValue("@AssignmentTypeDescription", assignmentType.AssignmentTypeDescription);
                        command.Parameters.AddWithValue("@DurationInSeconds", assignmentType.DurationInSeconds);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                }
            }  // Connection gets automatically closed here due to 'using' statement
        }

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
            }// Connection gets automatically closed here due to 'using' statement
        }

        public void InsertAssignmentData(List<Assignment> Assignments)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DayOfAssignment", typeof(string));
            dt.Columns.Add("TimeFrameStart", typeof(string));
            dt.Columns.Add("TimeFrameEnd", typeof(string));
            dt.Columns.Add("AssignmentTypeID", typeof(int));

            foreach (var assignment in Assignments)
            {
                DataRow row = dt.NewRow();
                row["DayOfAssignment"] = assignment.DayOfAssignment;
                row["TimeFrameStart"] = assignment.TimeFrameStart;
                row["TimeFrameEnd"] = assignment.TimeFrameEnd;
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
                        bulkCopy.ColumnMappings.Add("DayOfAssignment", "DayOfAssignment");
                        bulkCopy.ColumnMappings.Add("TimeFrameStart", "TimeFrameStart");
                        bulkCopy.ColumnMappings.Add("TimeFrameEnd", "TimeFrameEnd");
                        bulkCopy.ColumnMappings.Add("AssignmentTypeID", "AssignmentTypeID");

                        bulkCopy.DestinationTableName = "Assignment";
                        bulkCopy.WriteToServer(dt);
                    }
                    connection.Close();
                }

                Console.WriteLine($"\n{Assignments.Count} row(s) inserted.");
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
    }
}
