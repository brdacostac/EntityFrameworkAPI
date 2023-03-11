using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.DataBase
{
    public class RunePagesDb
    {
        public int Id
        {
            get; set;
        }

        [Required]
        [StringLength(50, ErrorMessage = "le nom runePage ne doit pas depasser 50 caractere")]
        public string Name
        {
            get; set;
        }



        public ICollection<CategoryDicDB> CategoryRunePages { get; set; } = new List<CategoryDicDB>();
    }
}
