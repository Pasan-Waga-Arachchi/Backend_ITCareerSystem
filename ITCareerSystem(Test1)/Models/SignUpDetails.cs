using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class SignUpDetails
    {
        [Key]
        public string? User_Name { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? TP_Number { get; set; }

       



    }
}
