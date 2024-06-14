using MySql.Data.MySqlClient;

namespace ElectricBillBooking.Models
{
    public class DbContext : IDisposable
    {
        private readonly MySqlConnection _mySqlConnection;

        public DbContext(string connectionString)
        {
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

        public void Dispose()
        {
            _mySqlConnection?.Dispose();
        }
    }
}
