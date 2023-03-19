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
            var champ = new Champion(championDb.Name, championDb.Class.ToChampionClass(), championDb.Icon, championDb.Image, championDb.Bio);
            if (championDb.Skins != null)
            {
                foreach(var skin in championDb.Skins)
                {
                    new Skin(skin.Name, champ, skin.Price, skin.Icon, skin.Image, skin.Description);
                }
            }

           championDb.Skills?.ToList().ForEach(s => champ.AddSkill( s.ToSkill()));
            if (championDb.caracteristics != null)
            {
                List<Tuple<string, int>> tuples = new();
                foreach (var caracteristic in championDb.caracteristics)
                {
                    champ.AddCharacteristics(new Tuple<string, int>(caracteristic.key, caracteristic.valeur));
                }
            }
            return champ;

        }

    }
}
