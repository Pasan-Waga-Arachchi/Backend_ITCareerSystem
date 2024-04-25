using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class AddNewDegree
    {
        //[Key]
        //[Required]
        //public String? Degree_ID { get; set; }
        //public String? DegreeName { get; set; }
        //[Required]
        //public String? Main_Discipline { get; set; }
        //public String? DegreeType { get; set;}
        //public String? Descp { get; set; }
        //public String? UniversityName { get; set; }  
        //public String? Faculty {get; set;}
        //public String? Department { get; set;}
        //public int? No_of_Chairs { get; set;}
        //public int? No_of_Credits { get; set;}
        //public String? Unicode { get; set;}
        //public String? Apptitute_Test { get; set;}

        
        public String? Degree_ID { get; set; }

        public String? Degree_Name { get; set; }
        public String? University_ID { get; set; }
        public String? No_of_Years { get; set; }
        public String? Industrial_Training { get; set; }
        public float? Credits { get; set; }
        public String? NVQ_SLQF { get; set; }
        public String? Degree_Type { get; set; }

        public String? Main_Discipline { get; set; }
        
        public int? No_of_Chairs { get; set; }
        public String? Faculty { get; set; }
        public String? Department { get; set; }
        public String? No_of_Special_Student { get; set; }
        public String? AptitudeTest { get; set; }

    }
}
