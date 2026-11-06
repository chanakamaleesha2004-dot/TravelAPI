using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly string _connectionString;

        public DestinationsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetDestinations()
        {
            List<Destination> destinations = new List<Destination>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Destinations, Categories සහ Districts වගු සම්බන්ධ කර දත්ත ලබාගැනීම
                string query = @"SELECT d.DestinationID, d.DestName, c.CategoryName, dist.DistrictName, d.Difficulty 
                                 FROM Destinations d
                                 INNER JOIN Categories c ON d.CategoryID = c.CategoryID
                                 INNER JOIN Districts dist ON d.DistrictID = dist.DistrictID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        destinations.Add(new Destination
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Category = reader.GetString(2),
                            District = reader.GetString(3),
                            Difficulty = reader.GetString(4)
                        });
                    }
                }
            }
            return Ok(destinations);
        }
    }
}