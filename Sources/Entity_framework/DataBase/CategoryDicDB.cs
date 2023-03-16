using Entity_framework.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.DataBase
{
    public class CategoryDicDB
    {
        public int Id
        {
            get; set;
        }

        [Required]
        public CategoryDb category { get; set; }

        public int runesPagesForeignKey
        {
            get; set;
        }

        [Required]
        public RunePagesDb runePage { get; set; }

        [Required]
        public RuneDB rune { get; set; }

    }
}
