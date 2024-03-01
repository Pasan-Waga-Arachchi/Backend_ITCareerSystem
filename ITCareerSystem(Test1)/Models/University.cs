using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITCareerSystem_Test1_.Models
{
    public class University
    {

        [Column(TypeName = "nvarchar(50)")]
        [Key] public required string University_ID { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? UniversityName { get; set; }

    }
}