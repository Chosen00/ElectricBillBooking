using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ElectricBillBooking.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DatabaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("TestConnection")]
        public IActionResult TestConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        return Ok(new { Message = "Successfully connected to the database" });
                    }
                    else
                    {
                        return BadRequest(new { Message = "Failed to connect to the database" });
                    }
                }
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while checking the database connection: {ex.Message}" });
            }
        }
    }
}
