using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class ALSubject
    {
        [Key]
        public String? Subject_ID { get; set; }

        public String? Subject_Name { get; set;}
    }
}
