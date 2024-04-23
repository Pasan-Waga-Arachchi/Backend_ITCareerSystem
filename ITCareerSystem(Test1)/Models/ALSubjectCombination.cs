
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ITCareerSystem_Test1_.Models
{
    public class ALSubjectCombination
    {

        [Column (TypeName ="char(10)")]
        [Key] public required string Combination_ID { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Combination {get; set; }
    }
}