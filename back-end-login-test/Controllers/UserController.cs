
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ITCareerSystem_Test1_.Data;
using ITCareerSystem_Test1_.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace ITCareerSystem_Test1_.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserDbContext userContext;

        public UserController(IConfiguration configuration, UserDbContext userContext)
        {
            this._configuration = configuration;
            this.userContext = userContext;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            // get data from the database
            var userDomainModel = userContext.User.ToList();

            //map domain models to dtos
            var userDtos = new List<UserDTO>();
            foreach (var uDTO in userDomainModel)
            {
                userDtos.Add(new UserDTO()
                {
                    User_Name = uDTO.User_Name,
                    Password = uDTO.Password,
                    Email = uDTO.Email,
                    TP_Number = uDTO.TP_Number,
                });
            }
            return Ok(userDtos);   
        }

        [HttpPost("Login")]
        public IActionResult CheckUsername(string user_Name, string password)
        {
            

            if (string.IsNullOrEmpty(user_Name) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username or Password cannot be empty");
            }

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
            {
                con.Open();
                string query = "SELECT * FROM [User] WHERE User_Name = @Username";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", user_Name);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve column values from the reader
                            
                            string userName = reader.GetString(reader.GetOrdinal("User_Name"));
                            string pwd = reader.GetString(reader.GetOrdinal("Password"));
                            string email = reader.GetString(reader.GetOrdinal("Email"));
                            string phone = reader.GetString(reader.GetOrdinal("TP_Number"));
                            // You can retrieve other columns as needed
                            
                            if(pwd == password)
                            {
                                var userDtos = new UserDTO();

                                userDtos.User_Name = userName;
                                userDtos.Password = password;
                                userDtos.TP_Number = phone;

                                return Ok(userDtos);

                            }
                            else
                            {
                                return BadRequest("Password incorrect");
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


        [Authorize]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest("Username is required.");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Password is required.");
            }

            if (userContext.ValidateUser(request.Username, request.Password))
            {
                return Ok("Login successful"); // Replace with a more meaningful response
            }

            return BadRequest("Invalid username or password.");
        }


    }
}