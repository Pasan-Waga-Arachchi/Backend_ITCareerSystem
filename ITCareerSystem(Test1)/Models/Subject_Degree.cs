using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class Subject_Degree
    {
        [Key]
        public String? Subject_ID { get; set; }

        [Key]
        public String? Degree_ID { get; set; }

    }
}
