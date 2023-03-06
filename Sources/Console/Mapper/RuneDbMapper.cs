
using Rune = Model.Rune;
using Entity_framework;
using BiblioMilieu.Mapper.EnumsMapper;

namespace BiblioMilieu
{
    public static class RuneDbMapper
    {
        public static RuneDB ToDb(this Rune rune)
        {
            RuneDB runeDb = new RuneDB()
            {
                Name = rune.Name,
                Description = rune.Description,
                Family = rune.Family.ToDb(),
                Icon = rune.Icon,
                Image = rune.Image.Base64
            };
            return runeDb;
        }

        public static Rune ToRune(this RuneDB runeDb)
        {
            return new Rune(runeDb.Name, runeDb.Family.ToRuneFamily(), runeDb.Icon, runeDb.Image, runeDb.Description);
        }        
    }
}
