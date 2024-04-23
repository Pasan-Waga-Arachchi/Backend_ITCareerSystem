using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class Degree_University
    {
        [Key]
        public String? Degree_ID { get; set; }

        [Key]
        public String? University_ID { get; set; }
        public String? No_of_Years { get; set; }
        public String? Industrial_Training { get; set; }
        public float? Credits { get; set; }
        public String? NVQ_SLQF { get; set; }
        public String? Degree_Type { get; set; }
        public String? Description { get; set; }
        public int? No_of_Chairs { get; set; }
        public String? Faculty { get; set; }
        public String? Department { get; set; }  
        public int? No_of_Special_Student { get; set; }
        public String? AptitudeTest { get; set; }    

    }
}
