using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITCareerSystem_Test1_.Models
{
    public class User
    {
        [Column (TypeName ="varchar(50)")]
        [Key] public string User_Name { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Password { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? TP_Number { get; set; }
    }
}
