using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class Degree_ZScore
    {
        [Key]
        public String? Degree_ID { get; set; }

        [Key]
        public String? University_ID { get; set; }
        public String? District_ID { get; set; }
        public float? Year_ago_ZScore { get; set; }
        public float? Two_Year_ago_ZScore { get; set; }
        public float? Prediction { get; set; }

    }
}
