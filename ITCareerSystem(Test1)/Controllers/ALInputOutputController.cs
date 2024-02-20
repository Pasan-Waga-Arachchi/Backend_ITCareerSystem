using ITCareerSystem_Test1_.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("ALInputPage")]
    [ApiController]
    public class ALInputOutputController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ALInputOutputController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost] // Changed to POST to accept input from user
        [Route("GetDegreesBySubjects")]
        public IActionResult GetDegreesBySubjects(String Subject1, String Subject2, String Subject3, String District, float ZScore)
        {
            try
            {
                if (string.IsNullOrEmpty(Subject1) || string.IsNullOrEmpty(Subject2) || string.IsNullOrEmpty(Subject3) || string.IsNullOrEmpty(District) || ZScore==0)
                {
                    return BadRequest("JobRole Cannot be Empty");
                }
                // Initialize SQL connection
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"SELECT
                degreedetails.DegreeName,
                university.UniversityName,
                degree_university.No_of_Years,
                degree_university.Credits,
                degree_university.NVQ_SLQF,
                degree_university.Degree_Type,
                degree_university.No_of_Chairs,
                degree_university.Faculty,
                degree_university.Department,
                degree_university.No_of_Special_Student,
                degree_university.AptitudeTest,
                degree_zscore.Year_ago_Zscore,
                degree_zscore.Two_Year_ago_Zscore,
                degree_zscore.Prediction
            FROM
                alsubject
            JOIN
                subject_degrees ON alsubject.Subject_ID = subject_degrees.Subject_ID
            JOIN
                degreedetails ON subject_degrees.Degree_ID = degreedetails.Degree_ID
            JOIN
                degree_university ON subject_degrees.Degree_ID = degree_university.Degree_ID
            JOIN
                university ON degree_university.University_ID = university.University_ID
            JOIN
                degree_zscore ON degreedetails.Degree_ID = degree_zscore.Degree_ID
            JOIN
                district ON degree_zscore.District_ID = district.District_ID
            WHERE
               alsubject.Subject_Name IN (@Subject1, @Subject2, @Subject3)
               AND
               district.DistrictName IN (@District)
            GROUP BY
                degreedetails.DegreeName,
                university.UniversityName,
                degree_university.No_of_Years,
                degree_university.Credits,
                degree_university.NVQ_SLQF,
                degree_university.Degree_Type,
                degree_university.No_of_Chairs,
                degree_university.Faculty,
                degree_university.Department,
                degree_university.No_of_Special_Student,
                degree_university.AptitudeTest,
                degree_zscore.Year_ago_Zscore,
                degree_zscore.Two_Year_ago_Zscore,
                degree_zscore.Prediction;";

                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        ALInputRequest request = new ALInputRequest();

                        //cmd.Parameters.AddWithValue("@subject", request.Subject);
                        //cmd.Parameters.AddWithValue("@district", request.District);
                        cmd.Parameters.AddWithValue("@Subject1", Subject1);
                        cmd.Parameters.AddWithValue("@Subject2", Subject2);
                        cmd.Parameters.AddWithValue("@Subject3", Subject3);
                        cmd.Parameters.AddWithValue("@District", District);
                        cmd.Parameters.AddWithValue("@ZScore", ZScore);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // If degrees found, return them
                            if (dt.Rows.Count > 0)
                            {
                                List<Degree> degrees = new List<Degree>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    University university = new University();
                                    Degree degree = new Degree
                                    {

                                        DegreeName = row["DegreeName"].ToString(),
                                        UniversityName = row["UniversityName"].ToString(),
                                        No_of_Years = Convert.ToInt32(row["No_of_Years"]),
                                        Credits = Convert.ToInt32(row["Credits"]),
                                        NVQ_SLQF =  row["NVQ_SLQF"].ToString(),
                                        Degree_Type = row["Degree_Type"].ToString(),
                                        No_of_Chairs = Convert.ToInt32(row["No_of_Chairs"]),
                                        Faculty = row["Faculty"].ToString(),
                                        Department = row["Department"].ToString(),
                                        No_of_Special_Student = Convert.ToInt32(row["No_of_Special_Student"]),
                                        AptitudeTest = row["AptitudeTest"].ToString(),

                                        // Add other properties as needed


                                    };
                                    degrees.Add(degree);

                                }
                                return Ok(degrees);
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
