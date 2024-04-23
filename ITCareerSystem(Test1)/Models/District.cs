using System.ComponentModel.DataAnnotations;

namespace ITCareerSystem_Test1_.Models
{
    public class District
    {
        [Key]
        public String? District_ID { get; set; }
        public String? DistrictName { get; set; }

    }
}
