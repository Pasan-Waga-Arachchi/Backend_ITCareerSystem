using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Data.Common;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("MoreDegreeInfor")]
    [ApiController]
    public class MoreDegreeInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseConnection _dbConnection;
        public MoreDegreeInfoController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._dbConnection = DatabaseConnection.Instance(configuration);
        }

            [HttpGet]
        [Route("MoreDegreeInformation")]

        public IActionResult MoreDegreeInformation(String DegreeName, String UniversityName)
        {
            try
            {
                if (String.IsNullOrEmpty(DegreeName) || String.IsNullOrEmpty(UniversityName))
                {
                    return BadRequest("Degree Name and University Name Cannot be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"SELECT DISTINCT
                                        DU.No_of_Years,
                                        DU.Industrial_Training,
                                        DU.Credits,
                                        DU.NVQ_SLQF,
                                        DU.Degree_Type,
                                        DU.No_of_Chairs,
                                        DU.Faculty,
                                        DU.Department,
                                        DU.No_of_Special_Student,
                                        DU.AptitudeTest,
                                        U.UniversityName,
                                        DD.DegreeName,
                                        DD.Main_Discipline
                                    FROM 
                                        (
                                            SELECT DISTINCT 
                                                DC.Degree_ID
                                            FROM 
                                                Degree_Content DC
                                            INNER JOIN 
                                                DegreeDetails DD ON DC.Degree_ID = DD.Degree_ID
                                            WHERE 
                                                DD.DegreeName = @DegreeName
                                        ) DC
                                    INNER JOIN 
                                        DegreeDetails DD ON DC.Degree_ID = DD.Degree_ID
                                    INNER JOIN 
                                        Degree_University DU ON DD.Degree_ID = DU.Degree_ID
                                    INNER JOIN 
                                        University U ON DU.University_ID = U.University_ID
                                    LEFT JOIN 
                                        Degree_Jobs DJ ON DD.Degree_ID = DJ.Degree_ID
                                    LEFT JOIN 
                                        Job_Career JC ON DJ.Job_ID = JC.Job_ID
                                    WHERE 
                                        U.UniversityName = @UniversityName;";

                    // Execute query
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
                                        moreDegree.DegreeName = row["DegreeName"].ToString();
                                        moreDegree.UniversityName = row["UniversityName"].ToString();
                                        moreDegree.No_of_Years = Convert.ToInt32(row["No_of_Years"]);
                                        moreDegree.No_of_Chairs = Convert.ToInt32(row["No_of_Chairs"]);
                                        moreDegree.AptitudeTest = row["AptitudeTest"].ToString();
                                        moreDegree.Credits = Convert.ToInt32(row["Credits"]);
                                        moreDegree.NVQ_SLQF = Convert.ToInt32(row["NVQ_SLQF"]);
                                        moreDegree.Degree_Type = row["Degree_Type"].ToString();
                                        moreDegree.Faculty = row["Faculty"].ToString();
                                        moreDegree.Department = row["Department"].ToString();
                                        moreDegree.No_of_Special_Student = Convert.ToInt32(row["No_of_Special_Student"]);
                                        //moreDegree.Level = Convert.ToInt32(row["Level"]);
                                        //moreDegree.Semester = Convert.ToInt32(row["Semester"]);
                                        //moreDegree.Subject = row["Subject"].ToString();
                                        //moreDegree.SubCredits = Convert.ToSingle(row["SubCredits"]);
                                        //moreDegree.Core_Optional = row["Core_Optional"].ToString();
                                        //moreDegree.Job_Name = row["Job_Name"].ToString();
                                        //moreDegree.Descp = row["Descp"].ToString();
                                        //moreDegree.Local_Global = row["Local_Global"].ToString();
                                        //moreDegree.Estimated_Salary = row["Estimated_Salary"].ToString();
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
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}