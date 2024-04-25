using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using System.Xml.Serialization;
using ITCareerSystem_Test1_.Models;

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
        public IActionResult ZScorePredictionDetails([FromForm] DegreeZScore degree, IFormFile csvFile)
        {
            
            String Degree_ID = degree.Degree_ID;
            String University_ID = degree.University_ID;
            String District_ID = degree.District_ID;
            decimal Year_ago_ZScore = degree.Year_ago_ZScore;
            decimal Two_Year_ago_ZScore = degree.Two_Year_ago_ZScore;
           

            try
            {
                if (string.IsNullOrEmpty(Degree_ID) || string.IsNullOrEmpty(University_ID) || string.IsNullOrEmpty(District_ID))
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
                    csvFile.CopyToAsync(stream);
                }


                string csvFilePath = filePath;
                PythonCode pythonCode = new PythonCode();
                string output = pythonCode.GetPythonValue(csvFilePath);
                string errorMessage = "Python script did not produce any output or encountered an error.";
                  return StatusCode(500, output);
                //decimal.TryParse(output, out decimal predictedValue);
                //if (output == null)
                //{
                //    string errorMessage = "Python script did not produce any output or encountered an error.";
                //    return StatusCode(500, errorMessage);
                //}
                //else
                //{
                //    using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                //    {

                //        con.Open();
                //        string query = @"UPDATE [Degree_ZScore] 
                //      SET Prediction = @Prediction, Year_ago_ZScore = @Year_ago_ZScore, Two_Year_ago_ZScore = @Two_Year_ago_ZScore
                //       WHERE Degree_ID = @Degree_ID AND University_ID = @University_ID AND District_ID = @District_ID;";
                //        //Execute Query
                //        using (SqlCommand cmd = new SqlCommand(query, con))
                //        {
                //            cmd.Parameters.AddWithValue("@Prediction", predictedValue);
                //            cmd.Parameters.AddWithValue("@Year_ago_ZScore", Year_ago_ZScore);
                //            cmd.Parameters.AddWithValue("@Two_Year_ago_ZScore", Two_Year_ago_ZScore);
                //            cmd.Parameters.AddWithValue("@Degree_ID", Degree_ID);
                //            cmd.Parameters.AddWithValue("@University_ID", University_ID);
                //            cmd.Parameters.AddWithValue("@District_ID", District_ID);
                //            int rowsAffected = cmd.ExecuteNonQuery();

                //            if (rowsAffected > 0)
                //            {
                //                return Ok("ZScore details inserted successfully");
                //            }
                //            else
                //            {
                //                return StatusCode(500, "Failed to insert z score details");
                //            }
                //        }
                //    }
                //    return Ok(new { Message = "File uploaded and data processed successfully" });
                //}

                //// Call Python script
                ////var processInfo = new ProcessStartInfo
                ////{
                ////    FileName = "python3",
                ////    Arguments = $"ZScorePrediction.py {filePath}",
                ////    RedirectStandardOutput = true,
                ////    UseShellExecute = false,
                ////    CreateNoWindow = true
                ////};


                ////using (var process = Process.Start(processInfo))
                ////{

                //    if (process != null)
                //    {

                //        process.WaitForExit();
                //        string output = process.StandardOutput.ReadToEnd().Trim();

                //        if (output == null)
                //        {
                //            string errorMessage = "Python script did not produce any output or encountered an error.";
                //            return StatusCode(500, errorMessage);
                //        }
                //        else if (decimal.TryParse(output, out decimal predictedValue))
                //        {
                //            //predictedValue = decimal.Parse(output);

                //            //using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DataBaseConnection")))
                //            //{
                //            //    con.Open();
                //            //    string query = @"UPDATE [Degree_ZScore] 
                //            //          SET Prediction = @Prediction, Year_ago_ZScore = @Year_ago_ZScore, Two_Year_ago_ZScore = @Two_Year_ago_ZScore
                //            //         WHERE Degree_ID = @Degree_ID AND University_ID = @University_ID AND District_ID = @District_ID;";

                //            //    //Execute Query
                //            //    using (SqlCommand cmd = new SqlCommand(query, con))
                //            //    {
                //            //        cmd.Parameters.AddWithValue("@Prediction", predictedValue);
                //            //        cmd.Parameters.AddWithValue("@Year_ago_ZScore", Year_ago_ZScore);
                //            //        cmd.Parameters.AddWithValue("@Two_Year_ago_ZScore", Two_Year_ago_ZScore);
                //            //        cmd.Parameters.AddWithValue("@Degree_ID", Degree_ID);
                //            //        cmd.Parameters.AddWithValue("@University_ID", University_ID);
                //            //        cmd.Parameters.AddWithValue("@District_ID", District_ID);
                //            //        int rowsAffected = cmd.ExecuteNonQuery();

                //            //        if (rowsAffected > 0)
                //            //        {
                //            //            return Ok("ZScore details inserted successfully");
                //            //        }
                //            //        else
                //            //        {
                //            //            return StatusCode(500, "Failed to insert z score details");
                //            //        }
                //            //    }
                //            //}

                //            return Ok(new { Message = "File uploaded and data processed successfully" });
                //        }
                //        else
                //        {
                //            string errorMessage = "Failed to parse predicted value from Python output.";
                //            errorMessage += $"\nPython Output: {output}";
                //            return StatusCode(500, errorMessage);
                //        }
                //    }
                //    else
                //    {
                //        return StatusCode(500, "Failed to start Python process.");
                //    }
                ////}
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}


