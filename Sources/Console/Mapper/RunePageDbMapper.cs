using Entity_framework.DataBase;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioMilieu.Mapper.EnumsMapper
{
    public static class RunePageDbMapper
    {
        public static RunePagesDb ToDb(this RunePage runePageModele)
        {
            RunePagesDb categoryDic = new RunePagesDb()
            {
               Name = runePageModele.Name
            };
            return categoryDic;
            
        }

        public static RunePage ToRunePage(this RunePagesDb runePageModele)
        {
           return new RunePage(runePageModele.Name);
        }



    }
}
