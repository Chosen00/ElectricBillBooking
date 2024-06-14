using MySql.Data.MySqlClient;
using System;

namespace ElectricBillBooking.Models
{
    public class SignUpContext
    {
        private readonly string _connectionString;

        public SignUpContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to check if username and password already exist
        public bool IsUserExists(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT COUNT(*) FROM clienttable WHERE UserName = @UserName AND Password = @Password";
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public bool InsertClient(SignUpModel client)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    // Insert client information into clienttable
                    command.CommandText = @"INSERT INTO clienttable (UserName, Password, LastName, FirstName, MiddleName, SuffixName, Birthdate, Gender, CellphoneNumber, StreetAddress, City, StateProvince) 
              VALUES (@UserName, @Password, @LastName, @FirstName, @MiddleName, @SuffixName, @Birthdate, @Gender, @CellphoneNumber, @StreetAddress, @City, @StateProvince)";

                    // Add parameters for client information
                    command.Parameters.AddWithValue("@UserName", client.UserName);
                    command.Parameters.AddWithValue("@Password", client.Password);
                    command.Parameters.AddWithValue("@LastName", client.LastName);
                    command.Parameters.AddWithValue("@FirstName", client.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", client.MiddleName);
                    command.Parameters.AddWithValue("@SuffixName", client.SuffixName);
                    command.Parameters.AddWithValue("@Birthdate", client.Birthdate);
                    command.Parameters.AddWithValue("@Gender", client.Gender);
                    command.Parameters.AddWithValue("@CellphoneNumber", client.CellphoneNumber);
                    command.Parameters.AddWithValue("@StreetAddress", client.StreetAddress);
                    command.Parameters.AddWithValue("@City", client.City);
                    command.Parameters.AddWithValue("@StateProvince", client.StateProvince);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        Console.WriteLine("Error inserting client. No rows affected.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Client information inserted successfully.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting client: {ex.Message}");
                return false;
            }
        }
    }
}
