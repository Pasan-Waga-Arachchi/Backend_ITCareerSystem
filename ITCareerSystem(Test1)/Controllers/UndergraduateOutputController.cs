using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Add this using directive
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("UndergraduateOutput")]
    [ApiController]
    public class UndergraduageOutputController : ControllerBase
    {
        private IConfiguration _configuration;

        public UndergraduageOutputController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUndergraduateOutputdetails")]

        public IActionResult GetUndergraduateOutputdetails(String UniversiryName, String DegreeName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(UniversiryName) || String.IsNullOrEmpty(DegreeName))
                {
                    return BadRequest("Values Cannot be Empty");
                }
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"
                                    SELECT 

                                        JC.Job_Name,
                                        JC.Descp,
                                        JC.Estimated_Salary,
                                        JC.Local_Global

                                    FROM 

                                        Job_Career JC
                                        INNER JOIN Degree_Jobs DJ ON JC.Job_Id = DJ.Job_Id
                                        INNER JOIN DegreeDetails DD ON DJ.Degree_ID = DD.Degree_ID
                                        INNER JOIN Degree_University DU ON DU.Degree_ID = DD.Degree_ID
                                        INNER JOIN University U ON U.University_ID = DU.University_ID
                                    WHERE 
                                        U.UniversityName = @UniversiryName
                                        AND DD.DegreeName = @DegreeName;";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@UniversiryName", UniversiryName);
                        cmd.Parameters.AddWithValue("@DegreeName", DegreeName);



                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // If degrees found, return them
                            if (dt.Rows.Count > 0)
                            {
                                List<UndergraduageOutput> undergraduateOutput = new List<UndergraduageOutput>();
                                foreach (DataRow row in dt.Rows)
                                {

                                    UndergraduageOutput undergraduage = new UndergraduageOutput();
                                    {

                                        undergraduage.Job_Name = row["Job_Name"].ToString();
                                        undergraduage.Descp = row["Descp"].ToString();
                                        undergraduage.Local_Global = row["Local_Global"].ToString();
                                        undergraduage.Estimated_Salary = row["Estimated_Salary"].ToString();


                                    };
                                    undergraduateOutput.Add(undergraduage);

                                }
                                return Ok(undergraduateOutput);
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
