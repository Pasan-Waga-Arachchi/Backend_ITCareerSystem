using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace ITCareerSystem_Test1_.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ZScorePredictorController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ZScorePredictorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("ZScorePredict")]
        public IActionResult ZScorePredictionDetails([FromBody] DegreeZScore degree)
        {
            String Degree_ID = degree.Degree_ID;
            String University_ID = degree.University_ID;
            String District_ID = degree.District_ID;
            decimal Year_ago_ZScore = degree.Year_ago_ZScore;
            decimal Two_Year_ago_ZScore = degree.Two_Year_ago_ZScore;
            decimal Prediction = degree.Prediction;

            try
            {
                if(string.IsNullOrEmpty(Degree_ID) || string.IsNullOrEmpty(University_ID) || string.IsNullOrEmpty(District_ID)) 
                {
                    return BadRequest("Degree ID, University ID, District ID cannot be null or empty.");
                }

                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                {
                    con.Open();
                    string query  = @"UPDATE [Degree_ZScore] 
                                      SET Prediction = @Prediction, Year_ago_ZScore = @Year_ago_ZScore, Two_Year_ago_ZScore = @Two_Year_ago_ZScore
                                     WHERE Degree_ID = @Degree_ID AND University_ID = @University_TD AND District_ID = @District_ID;"
                }

            }
            catch (Exception ex){
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
