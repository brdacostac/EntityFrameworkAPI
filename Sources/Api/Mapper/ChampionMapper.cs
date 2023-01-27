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
                Class=champion.Class.ToString()
            };
            return dTOChampion;
        }


    }
}
