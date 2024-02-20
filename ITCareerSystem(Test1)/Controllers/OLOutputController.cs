using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Add this using directive
using System.Data.SqlClient;
using System.Data;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("OLOutput")]
    [ApiController]
    public class OLOutputController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OLOutputController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost] // Changed to POST to accept input from user
        [Route("GetOLOutputDetails")]
        public IActionResult GetOLOutputDetails(string jobRole)
        {
            // Your code logic goes here


            try
            {
                if (string.IsNullOrEmpty(jobRole))
                {
                    return BadRequest("JobRole Cannot be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"SELECT
    ALSubjectCombination.Combination,
    DegreeDetails.DegreeName,
    University.UniversityName
FROM
    ALSubjectCombination
JOIN
    CombinationDegree ON ALSubjectCombination.Combination_ID = CombinationDegree.Combination_ID
JOIN
    degree_jobs ON CombinationDegree.Degree_ID = degree_jobs.Degree_ID
JOIN
    job_career ON degree_jobs.Job_Id = job_career.Job_Id
JOIN
    DegreeDetails ON CombinationDegree.Degree_ID = DegreeDetails.Degree_ID
JOIN
    Degree_University ON CombinationDegree.Degree_ID = Degree_University.Degree_ID
JOIN
    University ON Degree_University.University_ID = University.University_ID
WHERE
    job_career.Job_Name = @jobRole

";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@jobRole", jobRole);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // If degrees found, return them
                            if (dt.Rows.Count > 0)
                            {
                                List<OLOutput> olOutput = new List<OLOutput>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    
                                    OLOutput oLOutput = new OLOutput();
                                    {
                                        
                                        oLOutput.Combination = row["Combination"].ToString(); // Assign value to Combination property
                                        oLOutput.DegreeName = row["DegreeName"].ToString();
                                        oLOutput.UniversityName = row["UniversityName"].ToString() ;

                                    };
                                    olOutput.Add(oLOutput);

                                }
                                return Ok(olOutput);
                            }
                            else
                            {
                                return StatusCode(404, "No degrees found for the provided subjects.");
                            }
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
