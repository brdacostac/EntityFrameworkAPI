using DTOLol;
using Model;

namespace MapperApi.Mapper
{
    public static class RuneMapper
    {
        public static DTORune ToDto(this Rune rune)
        {
            DTORune dTORune = new DTORune()
            {
                Name = rune.Name,
                Family = rune.Family.ToString(),
                Description = rune.Description,
                Icon = rune.Icon,
                Image = rune.Image.Base64,
            };
            return dTORune;
        }
        
        
        public static Rune ToRune(this DTORune dTORune)
        {
            RuneFamily runeFamily = Enum.TryParse<RuneFamily>(dTORune.Family, true, out runeFamily) ? runeFamily : RuneFamily.Unknown;
            return new Rune(dTORune.Name, runeFamily, dTORune.Icon, dTORune.Image, dTORune.Description);
        }
        

    }
}
