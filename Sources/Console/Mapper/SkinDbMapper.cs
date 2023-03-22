using Entity_framework;
using Model;

namespace BiblioMilieu
{
    public static class SkinDbMapper
    {
        public static SkinDB ToDb(this Skin skin,ChampionDB champion=null)
        {
            SkinDB skinDb = new SkinDB()
            {
                Name = skin.Name,
                Description = skin.Description,
                Icon = skin.Icon,
                Image = skin.Image.Base64,
                Price = skin.Price,
                Champion = champion==null ? skin.Champion.ToDb() : champion,
            };
            return skinDb;
        }

        public static Skin ToSkin(this SkinDB skinDb)
        {
            return new Skin(skinDb.Name, skinDb.Champion.ToChampion(), skinDb.Price, skinDb.Icon, skinDb.Image, skinDb.Description);
        }

    }
}