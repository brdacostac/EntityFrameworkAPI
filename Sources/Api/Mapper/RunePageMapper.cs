using DTOLol;
using Model;
using static Model.RunePage;

namespace Api.Mapper
{
    public static class RunePageMapper
    {
        public static DTORunePage ToDto(this RunePage runePage)
        {

            Dictionary<string, DTORune> dtoDic = runePage.Runes.ToDictionary(r => r.Key.ToString(), r => r.Value.ToDto());
            DTORunePage dTORunePage = new DTORunePage()
            {
                Name = runePage.Name,
                DTORuneDic= dtoDic
            };
            return dTORunePage;
        }


        public static RunePage ToRunePage(this DTORunePage dTORune)
        {
            Category tmp;
           Dictionary<Category,Rune > runDico = dTORune.DTORuneDic.ToDictionary(
               r => (RunePage.Category)Enum.Parse(typeof(RunePage.Category), r.Key),
               r => r.Value.ToRune()
           );

            RunePage runePage = new RunePage(dTORune.Name);
            foreach (var rune in dTORune.DTORuneDic)
            {
                if(! Enum.TryParse<Category>(rune.Key, true, out tmp))
                {
                    continue;
                }
                runePage[tmp] = rune.Value.ToRune();
            }

            return runePage;

        }
    }
}
