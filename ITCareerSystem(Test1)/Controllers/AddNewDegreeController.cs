using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("AddNewDegree")]
    [ApiController]
    public class AddNewDegreeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AddNewDegreeController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpPost]
        [Route("PostAddNewDegree")]


        public IActionResult PostAddNewDegree(String Degree_ID, String DegreeName, String Main_Discipline)
        {
            try
            {
                if (String.IsNullOrEmpty(Degree_ID) || String.IsNullOrEmpty(DegreeName) || String.IsNullOrEmpty(Main_Discipline))
                {
                    return BadRequest("Values Cannot be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"INSERT INTO [DegreeDetails] (Degree_ID, DegreeName, Main_Discipline)
  VALUES (@Degree_ID, @DegreeName, @Main_Discipline)";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                       
                        cmd.Parameters.AddWithValue("@Degree_ID", Degree_ID);
                        cmd.Parameters.AddWithValue("@DegreeName", DegreeName);
                        cmd.Parameters.AddWithValue("@Main_Discipline", Main_Discipline);


                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("New Degree details inserted successfully");
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
                return BadRequest(ex);
            }

        }
        
    }
}
