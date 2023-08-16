using System.Linq;
using Microsoft.Data.SqlClient;
using RoutePlanner.Models;

namespace RoutePlanner
{
    internal class DBManagerTwo
    {
        // Connection string for the database. Make sure to replace with your credentials.
        private readonly string _connectionString = "Server=Tinko;Database=ComfortCare;User Id=sa;Password=Kode1234!;TrustServerCertificate=true";

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
    }
}
