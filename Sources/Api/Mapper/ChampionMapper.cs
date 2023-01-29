using DTOLol;
using Model;

namespace Api.Mapper
{
    public static class ChampionMapper
    {
        public static DTOChampion ToDto(this Champion champion)
        {
            DTOChampion dTOChampion = new DTOChampion()
            {
                Name = champion.Name,
                Bio= champion.Bio,
                Class=champion.Class.ToString(),
                Icon = champion.Icon,
                Image = champion.Image.Base64
            };
            return dTOChampion;
        }

        public static Champion ToChampion(this DTOChampion dTOchampion)
        {
            ChampionClass championClass = Enum.TryParse<ChampionClass>(dTOchampion.Class, true, out championClass) ? championClass : ChampionClass.Unknown;
            return new Champion(dTOchampion.Name, championClass,dTOchampion.Icon,dTOchampion.Image,dTOchampion.Bio);   
        }

    }
}
