using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_framework.DataBase
{
    public class ChampionRunePageDB
    {
        public int Id { get; set; }
        public int ChampionId { get; set; }
        public int RunePageId { get; set; }

        public ChampionDB Champion { get; set; }
        public RunePagesDb RunePage { get; set; }
    }
}
