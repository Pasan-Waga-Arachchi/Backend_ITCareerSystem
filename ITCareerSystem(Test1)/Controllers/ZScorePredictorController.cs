using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Diagnostics;

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
        public async Task<IActionResult> ZScorePredictionDetails([FromForm] DegreeZScore degree, IFormFile csvFile) 
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

                if (csvFile == null || csvFile.Length == 0)
                {
                    return BadRequest("CSV file is required.");
                }

                // Handle file upload
                var filePath = Path.GetTempFileName(); 

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await csvFile.CopyToAsync(stream);
                }


                // Call Python script
                var processInfo = new ProcessStartInfo
                {
                    FileName = "ZScorePrediction",
                    Arguments = $"ZScorePrediction.py {"C:/Users/hewaw/OneDrive/Desktop/FP16/project/BackEnd-Computing-Degree-Program-System/Backend_ITCareerSystem/ZScorePrediction.py"}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit();
                        var output = process.StandardOutput.ReadToEnd();
                        if (decimal.TryParse(output, out decimal predictedValue))
                        {
                            predictedValue = decimal.Parse(output);
                            Prediction = predictedValue;
                            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                            {
                                con.Open();
                                string query = @"UPDATE [Degree_ZScore] 
                                      SET Prediction = @Prediction, Year_ago_ZScore = @Year_ago_ZScore, Two_Year_ago_ZScore = @Two_Year_ago_ZScore
                                     WHERE Degree_ID = @Degree_ID AND University_ID = @University_TD AND District_ID = @District_ID;";
                            }

                            return Ok(new { Message = "File uploaded and data processed successfully" });
                        }
                        else
                        {
                            return StatusCode(500, "Failed to parse predicted value from Python output.");
                        }
                    }
                    else
                    {
                        return StatusCode(500, "Failed to start Python process.");
                    }
                }
            }
            catch (Exception ex){
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
