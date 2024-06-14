using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Data;

namespace ElectricBillBooking.Models
{
    public class BookingContext
    {
        private readonly MySqlConnection _mySqlConnection;
        private readonly string _connectionString;
        private readonly ILogger<BookingContext> _logger;
       


        public BookingContext(string connectionString, ILogger<BookingContext> logger)
        {   
            _mySqlConnection = new MySqlConnection(connectionString);
            _connectionString = connectionString;
            _logger = logger;

        }

        //Get Client ID by Fullname//
        public int GetClientIdByFullName(string lastname, string firstname, string middlename, string suffixname)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT ClientID FROM clienttable WHERE LastName = @LastName AND FirstName = @FirstName AND MiddleName = @MiddleName AND SuffixName = @SuffixName", connection);
                    command.Parameters.AddWithValue("@LastName", lastname);
                    command.Parameters.AddWithValue("@FirstName", firstname);
                    command.Parameters.AddWithValue("@MiddleName", middlename);
                    command.Parameters.AddWithValue("@SuffixName", suffixname);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ClientID by full name: {ex.Message}");
                return -1;
            }
        }

        //Date and Time Validation
        public bool IsBookingDateTimeUnique(DateTime bookingDateTime)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM booktable WHERE BookDate = @BookingDateTime", connection);
                    command.Parameters.AddWithValue("@BookingDateTime", bookingDateTime);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking booking date and time uniqueness: {ex.Message}");
                return false;
            }
        }


        //**********************************************************************MY CRUD OPERATION************************************************************************//

        //Create a Booking//
        public bool InsertBooking(BookingModel bookclient)
        {
            try
            {
                // Get the ClientID based on the provided full name
                int clientId = GetClientIdByFullName(bookclient.LastName, bookclient.FirstName, bookclient.MiddleName, bookclient.SuffixName);

                // Ensure a valid ClientID is obtained
                if (clientId <= 0)
                {
                    Console.WriteLine("Error: ClientID not found for the provided full name.");
                    return false;
                }

                // Insert the booking data into booktable
                using (var connection = new MySqlConnection(_connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"INSERT INTO booktable (ClientID, Email, LastName, FirstName, MiddleName, SuffixName, BookDate) 
                                    VALUES (@ClientID, @Email, @LastName, @FirstName, @MiddleName, @SuffixName, @BookDate)";

                    command.Parameters.AddWithValue("@ClientID", clientId);
                    command.Parameters.AddWithValue("@Email", bookclient.Email);
                    command.Parameters.AddWithValue("@LastName", bookclient.LastName);
                    command.Parameters.AddWithValue("@FirstName", bookclient.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", bookclient.MiddleName);
                    command.Parameters.AddWithValue("@SuffixName", bookclient.SuffixName);
                    command.Parameters.AddWithValue("@BookDate", bookclient.BookDate);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting booking: {ex.Message}");
                return false;
            }
        }


        //**************************************************************UPDATE********************************************************//
        // Get Booktable data
        public List<BookingModel> GetBooktable()
        {
            List<BookingModel> clientList = new List<BookingModel>();
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand(
                "SELECT * FROM booktable", _mySqlConnection);
            using (MySqlDataReader reader = command.ExecuteReader()) 
            {

                        while (reader.Read())
                        {
                            clientList.Add (new BookingModel
                            {
                                ClientID = reader.GetInt32("ClientID"),
                                Email = reader.GetString("Email"),
                                LastName = reader.GetString("LastName"),
                                FirstName = reader.GetString("FirstName"),
                                MiddleName = reader.GetString("MiddleName"),
                                SuffixName = reader.GetString("SuffixName"),
                                BookDate = reader.GetDateTime("BookDate")
                                
                            });
                        }
                    }
                 return clientList;
                }


        // Get client from booktable by ID
        public BookingModel GetClientById(int clientId)
        {
            BookingModel client = null;
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM booktable WHERE ClientID = @ClientID", _mySqlConnection);
            command.Parameters.AddWithValue("ClientID", clientId);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    client = new BookingModel
                    {
                        ClientID = reader.GetInt32("ClientID"),
                        Email = reader.GetString("Email"),
                        LastName = reader.GetString("LastName"),
                        FirstName = reader.GetString("FirstName"),
                        MiddleName = reader.GetString("MiddleName"),
                        SuffixName = reader.GetString("SuffixName"),
                        BookDate = reader.GetDateTime("BookDate")
                    };
                }
            }
            return client;
        }


        // Update Booktable Method
        public bool UpdateClient(BookingModel updateBooktable)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"UPDATE booktable SET Email = @Email, LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, SuffixName = @SuffixName, BookDate = BookDate WHERE ClientID = @ClientID", _mySqlConnection);
                command.Parameters.AddWithValue("@ClientID", updateBooktable.ClientID);
                command.Parameters.AddWithValue("@Email", updateBooktable.Email);
                command.Parameters.AddWithValue("LastName", updateBooktable.LastName);
                command.Parameters.AddWithValue("FirstName", updateBooktable.FirstName);
                command.Parameters.AddWithValue("MiddleName", updateBooktable.MiddleName);
                command.Parameters.AddWithValue("SuffixName", updateBooktable.SuffixName);
                command.Parameters.AddWithValue("BookDate", updateBooktable.BookDate);

                int rowsAffected = command.ExecuteNonQuery();
                _mySqlConnection.Close();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Client: {ex.Message}");
                return false;
            }
        }




        //Get the data of my booktable form my database//
        public List<BookingModel> GetBookings()
        {
            List<BookingModel> bookings = new List<BookingModel>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM booktable", connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookingModel booking = new BookingModel
                            {
                                ClientID = Convert.ToInt32(reader["ClientID"]),
                                Email = reader["Email"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                SuffixName = reader["SuffixName"].ToString(),
                                BookDate = Convert.ToDateTime(reader["BookDate"])
                            };
                            bookings.Add(booking);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting bookings: {ex.Message}");
            }

            return bookings;
        }


        //********************************************************************************DELETE****************************************************************************//
        public bool DeleteBookingByFullname(string LastName, string FirstName, string MiddleName, string SuffixName)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM booktable WHERE LastName = @LastName AND FirstName = @FirstName AND MiddleName = @MiddleName AND SuffixName = @SuffixName";
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@MiddleName", MiddleName);
                        command.Parameters.AddWithValue("@SuffixName", SuffixName);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting booking: {ex.Message}", ex);
                return false;
            }
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
    }
}
