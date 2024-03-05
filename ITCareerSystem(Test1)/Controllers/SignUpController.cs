using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("SignUp")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private IConfiguration _configuration;
        public SignUpController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpPost]
        [Route("PostSignUpDetails")]

        public IActionResult PostSignUpDetails(String UserName, String Password, String email, String TP_Number)
        {
            try
            {
                if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(email)){
                    return BadRequest("Values Can not be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"INSERT INTO [User] (User_Name, Password, Email, TP_Number, UserRole)
VALUES (@UserName, @Password, @mail, @TP_Number, 'User')";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@UserName", UserName);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@mail", email);
                        cmd.Parameters.AddWithValue("@TP_Number", TP_Number);
                        

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("User details inserted successfully");
                        }
                        else
                        {
                            return StatusCode(500, "Failed to insert user details");
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
