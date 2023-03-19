using Entity_framework.DataBase;
using Entity_framework.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework
{
    public class RuneDB
    {
        public int Id
        {
            get; set;
        }
        [StringLength(50, ErrorMessage = "le nom rube ne doit pas depasser 50 caractere")]
        [Required]
        public string Name
        {
            get; set;
        }
        [StringLength(1000, ErrorMessage = "le description rune ne doit pas depasser 1000 caractere")]
        public string Description 
        {
            get; set; 
        }
        public RuneFamilyDb Family
        {
            get; set;
        }
        public string Icon
        {
            get; set;
        }
        public ICollection<CategoryDicDB> runesPages { get; set; }

        public string? Image
        {
            get; set;
        }
    }
}
