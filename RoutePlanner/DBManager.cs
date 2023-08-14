using Microsoft.Data.SqlClient;
using OpenRouteServiceApp;

namespace RoutePlanner
{
    internal class DBManager
    {
        // Connection string for the database. Make sure to replace with your credentials.
        private readonly string connectionString = "Server=ComfortCare;Database=ComforCare;User Id=sa;Password=Kode1234!;";

        public void InsertEmployeeTypeData(List<EmployeeType> employeeTypes)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in employeeTypes)
                {
                    string sqlCommandText = "INSERT INTO EmployeeType (Title) VALUES (@Title)";

                    using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@Title", item.Title);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }

                }
                // SQL command to insert data into the table.
            }  // Connection gets automatically closed here due to 'using' statement
        }


        public void InsertDayTypeData(List<TypeOfDAy> dateTypes)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in dateTypes)
                {
                    string sqlCommandText = "INSERT INTO DayType (DayType) VALUES (@DayType)";

                    using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@Title", item.DayType);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                }
                // SQL command to insert data into the table.
            }  // Connection gets automatically closed here due to 'using' statement
        }


        public void InsertSkillData(List<Skill> skills)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in skills)
                {
                    string sqlCommandText = "INSERT INTO Skill (Title, SkillDescription) VALUES (@Title, SkillDescription)";

                    using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                    {
                        command.Parameters.AddWithValue("@Title", item.Title);
                        command.Parameters.AddWithValue("@SkillDescription", item.SkillDescription);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                }
                // SQL command to insert data into the table.
            }  // Connection gets automatically closed here due to 'using' statement
        }


        public void InsertUserData(int id, string name, string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL command to insert data into the table.
                string sqlCommandText = "INSERT INTO Users (Id, Name, Email) VALUES (@Id, @Name, @Email)";

                using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                {
                    command.Parameters.AddWithValue("@Title", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }  // Connection gets automatically closed here due to 'using' statement
        }
    }
}
