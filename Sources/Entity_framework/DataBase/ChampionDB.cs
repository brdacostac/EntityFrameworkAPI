using Entity_framework;
using Microsoft.EntityFrameworkCore;
using Model;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework
{
    public class ChampionDB
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [StringLength(50, ErrorMessage = "le nom champion ne doit pas depasser 50 caractere")]

        public string Name
        {
            get; set;
        }

        [StringLength(1000, ErrorMessage = "le bio champion ne doit pas depasser 1000 caractere")]
        public string Bio
        {
            get; set;
        }

        [Required]
        public ClassChampionDb Class
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
        public ICollection<SkinDB> Skins { get; set; } = new List<SkinDB>();
    }
}
