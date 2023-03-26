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
            var runePage= new RunePage(runePageModele.Name);
            if (runePageModele.CategoryRunePages != null)
            {
                foreach(var category in runePageModele.CategoryRunePages)
                {
                    runePage.Runes.Append(new KeyValuePair<RunePage.Category, Model.Rune>(category.category.ToRuneFamily(), category.rune.ToRune()));
                }
            }
            return runePage;
        }
    }
}
