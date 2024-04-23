using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class UserDTO
    {
        [Key] public string User_Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string? TP_Number { get; set; }
    }
}
