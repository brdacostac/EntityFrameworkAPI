using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.DataBase
{
    public class CaracteristicDb
    {
        public int Id { get; set; }
        public string key { get; set; }

        public int valeur { get; set; }
        public int ChampionForeignKey
        {
            get; set;
        }
        public ChampionDB champion { get; set; }
        
    }
}
