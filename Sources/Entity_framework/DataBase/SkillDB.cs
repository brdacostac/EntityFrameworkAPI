using Entity_framework.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework
{
    public class SkillDB
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [StringLength(50, ErrorMessage = "le nom skill ne doit pas depasser 50 caractere")]
        public string Name
        {
            get; set;
        }
        [Required]
        public SkillTypeSkillDb Type
        {
            get; set;
        }
        [StringLength(1000, ErrorMessage = "le description skill ne doit pas depasser 1000 caractere")]
        public string Description
        {
            get; set;
        }
    }
}
