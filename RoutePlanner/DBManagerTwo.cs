using System.Linq;
using Microsoft.Data.SqlClient;
using RoutePlanner.Models;
using System.Data;

namespace RoutePlanner
{
    internal class DBManagerTwo
    {
        // Connection string for the database. Make sure to replace with your credentials.
        private readonly string _connectionString = "Server=LAPTOP-P6H4N3E7;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true"; //Kent


        //private readonly string _connectionString = "Server=Tinko;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true";

        public void InsertTaskTypeData(List<TaskType> taskTypes)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // SQL command to insert data into the table.
                string sqlCommandText = "INSERT INTO TaskType (Title, TaskTypeDescription, DurationInSeconds) VALUES (@Title, @TaskTypeDescription, @DurationInSeconds)";

                foreach (TaskType taskType in taskTypes)
                {
                    using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@Title", taskType.Title);
                        command.Parameters.AddWithValue("@TaskTypeDescription", taskType.TaskTypeDescription);
                        command.Parameters.AddWithValue("@DurationInSeconds", taskType.DurationInSeconds);

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
    }
}
