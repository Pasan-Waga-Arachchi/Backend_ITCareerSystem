using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class UpdateDegree
    {
        [Key]
        [Required]
        public String? Degree_ID { get; set; }
        public String? DegreeName { get; set; }
        [Required]
        public String? Main_Discipline { get; set; }
        public String? DegreeType { get; set; }
        public String? Descp { get; set; }
        public String? UniversityName { get; set; }
        public String? Faculty { get; set; }
        public String? Department { get; set; }
        public int? No_of_Chairs { get; set; }
        public int? No_of_Credits { get; set; }
        public String? Unicode { get; set; }
        public String? Apptitute_Test { get; set; }
    }
}
