using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
<<<<<<< HEAD
using System.Data.Common;
=======
>>>>>>> main

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("SignUp")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IConfiguration _configuration;
<<<<<<< HEAD
        private readonly DatabaseConnection _dbConnection;
        public SignUpController(IConfiguration configuration)
        {

            this._configuration = configuration;
            this._dbConnection = DatabaseConnection.Instance(configuration);
=======
        public SignUpController(IConfiguration configuration)
        {

            _configuration = configuration;
>>>>>>> main

        }

        [HttpPost]
        [Route("PostSignUpDetails")]

<<<<<<< HEAD
        public IActionResult PostSignUpDetails([FromBody] User user)
        {
            String UserName = user.User_Name;
            String Password = user.Password;
            String email = user.Email;
            String TP_Number = user.TP_Number;

            try
            {
                if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(email))
                {
=======
        public IActionResult PostSignUpDetails(String UserName, String Password, String email, String TP_Number)
        {
            try
            {
                if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(email)){
>>>>>>> main
                    return BadRequest("Values Can not be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"INSERT INTO [User] (User_Name, Password, Email, TP_Number, UserRole)
<<<<<<< HEAD
                        VALUES (@UserName, @Password, @email, @TP_Number, 'User')";
=======
VALUES (@UserName, @Password, @mail, @TP_Number, 'User')";
>>>>>>> main

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
<<<<<<< HEAD

=======
                        
>>>>>>> main
                        cmd.Parameters.AddWithValue("@UserName", UserName);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@mail", email);
                        cmd.Parameters.AddWithValue("@TP_Number", TP_Number);
<<<<<<< HEAD

=======
                        
>>>>>>> main

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
<<<<<<< HEAD
            catch (Exception ex)
=======
            catch (Exception ex) 
>>>>>>> main
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> main
