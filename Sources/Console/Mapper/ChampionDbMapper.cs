using Model;
using Entity_framework;
using BiblioMilieu.Mapper.EnumsMapper;

namespace BiblioMilieu
{
    public static class ChampionDbMapper
    {
        public static ChampionDB ToDb(this Champion champion)
        {
            ChampionDB championDb = new ChampionDB()
            {
                Name = champion.Name,
                Bio = champion.Bio,
                Class = champion.Class.ToDb(),
                Icon = champion.Icon,
                Image = champion.Image.Base64,
            };
            return championDb;
        }

        public static Champion ToChampion(this ChampionDB championDb)
        {
            return new Champion(championDb.Name, championDb.Class.ToChampionClass(), championDb.Icon, championDb.Image, championDb.Bio);
        }

    }
}
