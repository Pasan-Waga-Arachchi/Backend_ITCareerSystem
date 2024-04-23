<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> main

namespace ITCareerSystem_Test1_.Models
{
    public class ALSubjectCombination
    {
<<<<<<< HEAD

        [Column (TypeName ="char(10)")]
        [Key] public required string Combination_ID { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Combination {get; set; }
=======
        [Key]
        public String? Combination_ID { get; set; }
        public String? Combination {get; set; }
>>>>>>> main
    }
}