using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITCareerSystem_Test1_.Models
{
    public class DegreeDetails
    {

        [Column (TypeName ="char(10)")]
        [Key] public required string Degree_ID { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string DegreeName {get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string Main_Discipline {get; set; }
    }
}