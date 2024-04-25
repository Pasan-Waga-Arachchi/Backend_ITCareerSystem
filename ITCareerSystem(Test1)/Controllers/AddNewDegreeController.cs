using ITCareerSystem_Test1_.Models;
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


        public IActionResult PostAddNewDegree([FromBody] Degree_University newDegree)
        {
            
            
            String Degree_ID = newDegree.Degree_ID;
            String Main_Discipline = newDegree.Description;
            String University_ID = newDegree.University_ID;
            String No_of_Years = newDegree.No_of_Years;
            String Industrial_Training = newDegree.Industrial_Training;
            float Credits = (float)newDegree.Credits;
            String NVQ_SLQF = newDegree.NVQ_SLQF;
            String Degree_Type = newDegree.Degree_Type;
            int No_of_Chairs = (int)newDegree.No_of_Chairs;
            String Faculty = newDegree.Faculty;
            String Department = newDegree.Department;
            String AptitudeTest = newDegree.AptitudeTest;
            int No_of_Special_Student = (int)newDegree.No_of_Special_Student;
            try
            {

                if (String.IsNullOrEmpty(Degree_ID)  || String.IsNullOrEmpty(Main_Discipline) || String.IsNullOrEmpty(University_ID))

                if (String.IsNullOrEmpty(Degree_ID)  || String.IsNullOrEmpty(Main_Discipline) ||String.IsNullOrEmpty(University_ID))

                {
                    return BadRequest("Values Cannot be Empty");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();

                    // Select all degrees based on the provided subjects
                    // sql query to Add new degree to DegreeDetails 
                    string query = @"BEGIN TRANSACTION;

                        INSERT INTO [DegreeDetails] (Degree_ID, Main_Discipline)
                        VALUES (@Degree_ID, @Main_Discipline);

                        INSERT INTO Degree_University (Degree_ID, University_ID, No_of_Years, Industrial_Training, Credits, NVQ_SLQF, Degree_Type, No_of_Chairs, Faculty, Department, No_of_Special_Student, AptitudeTest)
                        VALUES (@Degree_ID, @University_ID, @No_of_Years, @Industrial_Training, @Credits, @NVQ_SLQF, @Degree_Type, @No_of_Chairs, @Faculty, @Department, @No_of_Special_Student, @AptitudeTest);

                        COMMIT;";


                    // Execute query
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {


                        cmd.Parameters.AddWithValue("@Degree_ID", Degree_ID);
                        cmd.Parameters.AddWithValue("@Main_Discipline", Main_Discipline);
                        cmd.Parameters.AddWithValue("@University_ID", University_ID);
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