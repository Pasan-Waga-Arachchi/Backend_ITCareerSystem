using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("Login")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration config)
        {
            _configuration = config;
        }

        //[HttpGet]

        //public IActionResult GetAll()
        //{
        //    // get data from the database
        //    var walert = "This data is coming through !!";

        //    //map domain models to dtos
            


        //    return Ok(walert);
        //}

        [HttpGet]
        public IActionResult CheckUsername(string username,string passwd)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwd))
            {
                return BadRequest("Username and password cannot be empty");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
            {
                con.Open();
                string query = "SELECT * FROM [User] WHERE [User_Name] = @Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string passwordHashFromDb = reader.GetString(reader.GetOrdinal("Password"));

                            // Verify the password using a secure method, e.g., hashing
                            // For demonstration purposes, we'll compare plaintext passwords
                            if (passwordHashFromDb == passwd)
                            {
                                return Ok("Login Successful");
                            }
                            else
                            {
                                return BadRequest("Invalid username or password");
                            }
                        }
                        else
                        {
                            return NotFound("Username does not exist");
                        }
                    }
                }
            }
        }
    }
}

