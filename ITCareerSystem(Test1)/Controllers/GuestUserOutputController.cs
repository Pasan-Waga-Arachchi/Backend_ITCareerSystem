using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("GuestUserOutput")]
    [ApiController]
    public class GuestUserOutputController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public GuestUserOutputController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpGet] // Changed to POST to accept input from user
        [Route("GetGuestUserOutput")]


        public IActionResult GetGuestUserOutput(String? UniversityName = null, int No_of_Years = 0, String? Combination = null, String? Degree_Type = null, int NVQ_SLQF = 0)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"

        -- Check if UniversityName is provided
        IF @UniversityName IS NOT NULL
        BEGIN
            -- Only UniversityName provided
            SELECT
                dd.DegreeName,
                u.UniversityName,
                du.No_of_Years,
                du.Degree_Type,
                du.NVQ_SLQF
            FROM
                DegreeDetails dd
            JOIN
                Degree_University du ON dd.Degree_ID = du.Degree_ID
            JOIN
                University u ON du.University_ID = u.University_ID
            WHERE
                u.UniversityName = @UniversityName;
        END;

        -- Check if No_of_Years is provided
        IF @No_of_Years IS NOT NULL
        BEGIN
            -- Only No_of_Years provided
            SELECT
                dd.DegreeName,
                u.UniversityName,
                du.No_of_Years,
                du.Degree_Type,
                du.NVQ_SLQF
            FROM
                DegreeDetails dd
            JOIN
                Degree_University du ON dd.Degree_ID = du.Degree_ID
            JOIN
                University u ON du.University_ID = u.University_ID
            WHERE
                du.No_of_Years = @No_of_Years;
        END;

        -- Check if Combination is provided
        IF @Combination IS NOT NULL
        BEGIN
            -- Only Combination provided
            SELECT
                dd.DegreeName,
                u.UniversityName,
                du.No_of_Years,
                du.Degree_Type,
                du.NVQ_SLQF
            FROM
                DegreeDetails dd
            JOIN
                Degree_University du ON dd.Degree_ID = du.Degree_ID
            JOIN
                University u ON du.University_ID = u.University_ID
            JOIN
                CombinationDegree cd ON dd.Degree_ID = cd.Degree_ID
            JOIN
                ALSubjectCombination ac ON cd.Combination_ID = ac.Combination_ID
            WHERE
                ac.Combination = @Combination;
        END;

        -- Check if Degree_Type is provided
        IF @Degree_Type IS NOT NULL
        BEGIN
            -- Only Degree_Type provided
            SELECT
                dd.DegreeName,
                u.UniversityName,
                du.No_of_Years,
                du.Degree_Type,
                du.NVQ_SLQF
            FROM
                DegreeDetails dd
            JOIN
                Degree_University du ON dd.Degree_ID = du.Degree_ID
            JOIN
                University u ON du.University_ID = u.University_ID
            WHERE
                du.Degree_Type = @Degree_Type;
        END;

        -- Check if NVQ_SLQF is provided
        IF @NVQ_SLQF IS NOT NULL
        BEGIN
            -- Only NVQ_SLQF provided
            SELECT
                dd.DegreeName,
                u.UniversityName,
                du.No_of_Years,
                du.Degree_Type,
                du.NVQ_SLQF
            FROM
                DegreeDetails dd
            JOIN
                Degree_University du ON dd.Degree_ID = du.Degree_ID
            JOIN
                University u ON du.University_ID = u.University_ID
            WHERE
                du.NVQ_SLQF = @NVQ_SLQF;
        END;";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (UniversityName != null)
                        {
                            cmd.Parameters.AddWithValue("@UniversityName", UniversityName);
                        }
                        else
                        {
                            // If UniversityName is not provided, use DBNull.Value
                            cmd.Parameters.AddWithValue("@UniversityName", DBNull.Value);
                        }

                        // Add other parameters similarly
                        if (No_of_Years != 0)
                        {
                            cmd.Parameters.AddWithValue("@No_of_Years", No_of_Years);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@No_of_Years", DBNull.Value);
                        }


                        if (Combination != null)
                        {
                            cmd.Parameters.AddWithValue("@Combination", Combination);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Combination", DBNull.Value);
                        }

                        if (Degree_Type != null)
                        {
                            cmd.Parameters.AddWithValue("@Degree_Type", Degree_Type);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Degree_Type", DBNull.Value);
                        }
                        if (NVQ_SLQF != 0)
                        {
                            cmd.Parameters.AddWithValue("@NVQ_SLQF", NVQ_SLQF);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@NVQ_SLQF", DBNull.Value);
                        }


                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // If degrees found, return them
                            if (dt.Rows.Count > 0)
                            {
                                List<GuestUserOutput> guestUserOutputs = new List<GuestUserOutput>();
                                foreach (DataRow row in dt.Rows)
                                {

                                    GuestUserOutput guestUser = new GuestUserOutput();
                                    {

                                        //oLOutput.Combination = row["Combination"].ToString(); // Assign value to Combination property
                                        //oLOutput.DegreeName = row["DegreeName"].ToString();
                                        //oLOutput.UniversityName = row["UniversityName"].ToString();
                                        guestUser.DegreeName = row["DegreeName"].ToString();
                                        guestUser.Universityname = row["Universityname"].ToString();
                                        guestUser.No_of_Years = Convert.ToInt32(row["No_of_Years"]);
                                        guestUser.NVQ_SLQF = Convert.ToInt32(row["NVQ_SLQF"]);

                                    };
                                    guestUserOutputs.Add(guestUser);

                                }
                                return Ok(guestUserOutputs);
                            }
                            else
                            {
                                return StatusCode(404, "No degrees found for the provided Details.");
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