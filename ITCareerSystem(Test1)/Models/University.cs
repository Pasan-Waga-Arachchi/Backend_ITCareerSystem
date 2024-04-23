<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> main

namespace ITCareerSystem_Test1_.Models
{
    public class University
    {
<<<<<<< HEAD

        [Column(TypeName = "nvarchar(50)")]
        [Key] public string University_ID { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? UniversityName { get; set; }
=======
        [Key]
        public String? University_ID { get; set; }
        public String? UniversityName { get; set; }
>>>>>>> main

    }
}