using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class ALSubjectCombination
    {
        [Key]
        public String? Combination_ID { get; set; }
        public String? Combination {get; set; }
    }
}
