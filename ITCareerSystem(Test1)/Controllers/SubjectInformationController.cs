using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Mvc;
using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("DegreeInfo")]
    [ApiController]
    public class SubjectInformationController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        private readonly DatabaseConnection _dbConnection;

        public SubjectInformationController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._dbConnection = DatabaseConnection.Instance(configuration);
        }

        [HttpGet]
        [Route("DegreeSubjectInformation")]
        public IActionResult MoreDegreeInformation(String DegreeName, String UniversityName)
        {
            try
            {
                if(String.IsNullOrEmpty(DegreeName) || String.IsNullOrEmpty(UniversityName))
                {
                    return BadRequest("Degree Name and University empty");
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                    {
                        con.Open();


                        string query = @"SELECT 
                                DC.Level,
                                DC.Semester,
                                DC.Subject,
                                DC.Core_Optional,
                                DC.SubCredits
                            FROM 
                                Degree_Content DC
                            INNER JOIN 
                                DegreeDetails DD ON DC.Degree_ID = DD.Degree_ID
                            INNER JOIN 
                                Degree_University DU ON DD.Degree_ID = DU.Degree_ID
                            INNER JOIN 
                                University U ON DU.University_ID = U.University_ID
                            WHERE 
                                DD.DegreeName = @DegreeName
                                AND U.UniversityName = @UniversityName;";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@DegreeName", DegreeName);
                            cmd.Parameters.AddWithValue("@UniversityName", UniversityName);

                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                adapter.Fill(dt);

                                // If degrees found, return them
                                if (dt.Rows.Count > 0)
                                {
                                    List<MoreDegreeInformation> moreDegreeInformatins = new List<MoreDegreeInformation>();
                                    foreach (DataRow row in dt.Rows)
                                    {

                                        MoreDegreeInformation moreDegree = new MoreDegreeInformation();
                                        {
                                            
                                            moreDegree.Level = Convert.ToInt32(row["Level"]);
                                            moreDegree.Semester = Convert.ToInt32(row["Semester"]);
                                            moreDegree.Subject = row["Subject"].ToString();
                                            moreDegree.SubCredits = Convert.ToSingle(row["SubCredits"]);
                                            moreDegree.Core_Optional = row["Core_Optional"].ToString();
                                           
                                        };
                                        moreDegreeInformatins.Add(moreDegree);

                                    }
                                    return Ok(moreDegreeInformatins);
                                }
                                else
                                {
                                    return StatusCode(404, "No degrees found for the provided subjects.");
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
