using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace ITCareerSystem_Test1_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateDegreeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UpdateDegreeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("UpdateDegreeDetails")]
        public IActionResult UpdateDegreeDetails(string DegreeName, string UniversityName, string Faculty, string Department, int No_of_Years, string Main_Discipline, string Industrial_Training, int Credits, int NVQ_SLQF, string Degree_Type, int No_of_Chairs, int No_of_Special_Student, string AptitudeTest)
        {
            try
            {
                if (string.IsNullOrEmpty(DegreeName) || string.IsNullOrEmpty(UniversityName) || string.IsNullOrEmpty(Faculty) || string.IsNullOrEmpty(Department))
                {
                    return BadRequest("Values cannot be empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    string updateDegreeDetailsQuery = @"UPDATE DegreeDetails 
                                                       SET Main_Discipline = @Main_Discipline
                                                       WHERE DegreeName = @DegreeName";

                    using (SqlCommand cmd = new SqlCommand(updateDegreeDetailsQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@DegreeName", DegreeName);
                        cmd.Parameters.AddWithValue("@Main_Discipline", Main_Discipline);

                        int rowsAffectedDegreeDetails = cmd.ExecuteNonQuery();

                        if (rowsAffectedDegreeDetails == 0)
                        {
                            return NotFound("Degree not found");
                        }
                    }

                    string updateDegreeUniversityQuery = @"UPDATE Degree_University 
                                                          SET No_of_Years = @No_of_Years,
                                                              Industrial_Training = @Industrial_Training,
                                                              Credits = @Credits,
                                                              NVQ_SLQF = @NVQ_SLQF,
                                                              Degree_Type = @Degree_Type,
                                                              No_of_Chairs = @No_of_Chairs,
                                                              No_of_Special_Student = @No_of_Special_Student,
                                                              AptitudeTest = @AptitudeTest
                                                          WHERE DegreeName = @DegreeName 
                                                          AND UniversityName = @UniversityName 
                                                          AND Faculty = @Faculty 
                                                          AND Department = @Department";

                    using (SqlCommand cmd = new SqlCommand(updateDegreeUniversityQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@DegreeName", DegreeName);
                        cmd.Parameters.AddWithValue("@UniversityName", UniversityName);
                        cmd.Parameters.AddWithValue("@No_of_Years", No_of_Years);
                        cmd.Parameters.AddWithValue("@Industrial_Training", Industrial_Training);
                        cmd.Parameters.AddWithValue("@Credits", Credits);
                        cmd.Parameters.AddWithValue("@NVQ_SLQF", NVQ_SLQF);
                        cmd.Parameters.AddWithValue("@Degree_Type", Degree_Type);
                        cmd.Parameters.AddWithValue("@No_of_Chairs", No_of_Chairs);
                        cmd.Parameters.AddWithValue("@Faculty", Faculty);
                        cmd.Parameters.AddWithValue("@Department", Department);
                        cmd.Parameters.AddWithValue("@No_of_Special_Student", No_of_Special_Student);
                        cmd.Parameters.AddWithValue("@AptitudeTest", AptitudeTest);

                        int rowsAffectedDegreeUniversity = cmd.ExecuteNonQuery();

                        if (rowsAffectedDegreeUniversity == 0)
                        {
                            return NotFound("Degree not found for the provided University, Faculty, or Department");
                        }
                    }

                    return Ok("Degree details updated successfully");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}