using Entity_framework;
using Entity_framework.DataBase;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioMilieu.Mapper
{
    public static class ChampionRunePageDbMapper
    {
        public static List<ChampionRunePageDB> ToChampionRunePageDBList(List<Tuple<Champion, RunePage>> championsAndRunePages)
        {
            var championRunePageDBList = new List<ChampionRunePageDB>();
            foreach (var tuple in championsAndRunePages)
            {
                var championRunePageDB = new ChampionRunePageDB
                {
                    Champion = new ChampionDB
                    {
                        Name = tuple.Item1.Name,
                        Bio = tuple.Item1.Bio,
                        Class = (ClassChampionDb)tuple.Item1.Class,
                        Icon = tuple.Item1.Icon,
                        Image = tuple.Item1.Image.Base64
                    },
                    RunePage = new RunePagesDb
                    {
                        Name = tuple.Item2.Name
                    }
                };
                championRunePageDBList.Add(championRunePageDB);
            }
            return championRunePageDBList;
        }

        public static List<Tuple<Champion, RunePage>> MapRunePagesDbToChampionsAndRunePages(List<RunePagesDb> runePagesDbs)
        {
            List<Tuple<Champion, RunePage>> championsAndRunePages = new List<Tuple<Champion, RunePage>>();

            foreach (var runePageDb in runePagesDbs)
            {
                foreach (var categoryRunePage in runePageDb.CategoryRunePages)
                {
                    //foreach (var rune in categoryRunePage.Runes)
                    //{
                    //    RunePage runePage = new RunePage(runePageDb.Name, categoryRunePage.Name, new List<Rune>() { new Rune(rune.Name, rune.Description) });
                    //    Champion champion = MapChampionDbToChampion(rune.Champion);
                    //    championsAndRunePages.Add(Tuple.Create(champion, runePage));
                    //}
                }
            }

            return championsAndRunePages;
        }
    }
}
