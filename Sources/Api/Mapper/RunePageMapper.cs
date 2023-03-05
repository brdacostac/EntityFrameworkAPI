using DTOLol;
using Model;

namespace Api.Mapper
{
    public static class RunePageMapper
    {
        public static DTORunePage ToDto(this RunePage runePage)
        {
            DTORunePage dTORunePage = new DTORunePage()
            {
                Name = runePage.Name,
            };

            return dTORunePage;
        }


        public static RunePage ToRune(this DTORunePage dTORune)
        {
            RuneFamily runeFamily = Enum.TryParse<RuneFamily>(dTORune.Family, true, out runeFamily) ? runeFamily : RuneFamily.Unknown;
            return new Rune(dTORune.Name, runeFamily, dTORune.Icon, dTORune.Image, dTORune.Description);
        }
    }
}
