using MySql.Data.MySqlClient;
using System;

namespace ElectricBillBooking.Models
{
    public class SignInContext
    {
        private readonly MySqlConnection _mySqlConnection;
        private readonly string _connectionString;

        public SignInContext(string connectionString)
        {
            _connectionString = connectionString;
            _mySqlConnection = new MySqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                _mySqlConnection.Open();
                _mySqlConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ValidateUser(string username, string password, out string errorMessage)
        {
            errorMessage = null;
            bool isValid = false;

            try
            {
                _mySqlConnection.Open();
                string query = "SELECT COUNT(*) FROM clienttable WHERE UserName = @Username AND Password = @Password";
                using (MySqlCommand cmd = new MySqlCommand(query, _mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    isValid = count > 0;
                }
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred while validating the user.";
                Console.WriteLine("Error validating user: " + ex.Message);
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return isValid;
        }

        public int GetClientIdByUsernameAndPassword(string username, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT ClientID FROM clienttable WHERE UserName = @Username AND Password = @Password";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
                return 0; // Return 0 if no ClientID found (possibly invalid username/password)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ClientID by username and password: {ex.Message}");
                return -1; // Return -1 to indicate an error
            }
        }

    }
}
