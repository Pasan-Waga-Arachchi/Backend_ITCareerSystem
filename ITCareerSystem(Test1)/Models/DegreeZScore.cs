using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DegreeZScore
{
    [Key]
    [Column(TypeName = "varchar(10)")]
    public string Degree_ID { get; set; }

    [Required]
    [Column(TypeName = "varchar(10)")]
    public string University_ID { get; set; }

    [Required]
    [Column(TypeName = "varchar(10)")]
    public string District_ID { get; set; }

    [Required]
    [Column(TypeName = "decimal(9,4)")]
    public decimal Year_ago_ZScore { get; set; }

    [Required]
    [Column(TypeName = "decimal(9,4)")]
    public decimal Two_Year_ago_ZScore { get; set; }

    [Required]
    [Column(TypeName = "decimal(9,4)")]
    public decimal Prediction { get; set; }

    public IFormFile CsvFilePath { get; set; } // Add this property
}
