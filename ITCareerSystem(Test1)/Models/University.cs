using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class University
    {
        [Key]
        public String? University_ID { get; set; }
        public String? UniversityName { get; set; }

    }
}
