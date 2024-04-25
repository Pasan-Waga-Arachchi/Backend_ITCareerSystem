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
        private readonly DatabaseConnection _dbConnection;

        public ALInputOutputController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._dbConnection = DatabaseConnection.Instance(configuration);
        }


            [HttpGet] // Changed to POST to accept input from user
        [Route("GetDegreesBySubjects")]
        public IActionResult GetDegreesBySubjects(String Subject1, String Subject2, String Subject3, String District, float ZScore)
        {
            try
            {
                if (string.IsNullOrEmpty(Subject1) || string.IsNullOrEmpty(Subject2) || string.IsNullOrEmpty(Subject3) || string.IsNullOrEmpty(District))
                {
                    return BadRequest("Values Cannot be Empty");
                }
                // Initialize SQL connection
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    string query = @"SELECT
                degreedetails.DegreeName,
                university.UniversityName,
                degree_zscore.Year_ago_Zscore,
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
                degree_zscore.Year_ago_Zscore,
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
                                List<ALInputOutput> aLInputOutputs = new List<ALInputOutput>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    ALInputOutput aOutput = new ALInputOutput();

                                    {

                                        aOutput.UniversityName = row["UniversityName"].ToString();
                                        aOutput.DegreeName = row["Degreename"].ToString();
                                        if (row["Year_ago_ZScore"] != DBNull.Value)
                                        {
                                            aOutput.Year_ago_ZScore = (float)Convert.ToDouble(row["Year_ago_ZScore"]);
                                        }

                                        if (row["Prediction"] != DBNull.Value)
                                        {
                                            aOutput.Prediction = (float?)Convert.ToDouble(row["Prediction"]);
                                        }



                                    };
                                    aLInputOutputs.Add(aOutput);

                                }
                                return Ok(aLInputOutputs);
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