using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework
{
    public class SkinDB
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [StringLength(50, ErrorMessage = "le nom skin ne doit pas depasser 50 caractere")]
        public string Name
        {
            get; set;
        }
        [StringLength(1000, ErrorMessage = "le description skin ne doit pas depasser 1000 caractere")]
        public string Description 
        {
            get; set;
        }
        public string Icon
        {
            get; set;
        }
        public string? Image
        {
            get; set;
        }
        [Range(0, float.MaxValue, ErrorMessage = "le prix skin ne doit pas etre négatif ou il depasse le float.max")]
        public float Price
        {
            get; set;
        }

        public int ChampionForeignKey
        {
            get; set;
        }
        [Required]
        [ForeignKey("ChampionForeignKey")]
        public ChampionDB Champion
        {
            get; set;
        }


        

    }
}
