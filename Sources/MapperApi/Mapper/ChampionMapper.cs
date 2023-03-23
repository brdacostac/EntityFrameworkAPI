using DTOLol;
using Model;


namespace MapperApi.Mapper
{
    public static class ChampionMapper
    {
        public static DTOChampion ToDto(this Champion champion)
        {
            DTOChampion dTOChampion = new DTOChampion()
            {
                Name = champion.Name,
                Bio = champion.Bio,
                Class = champion.Class.ToString(),
                Icon = champion.Icon,
                Image = champion.Image.Base64,
                Skills = champion.Skills.Select(s => s.ToDto()),
                Skins = champion.Skins.Select(s => s.ToDto()),
                Characteristics = champion.Characteristics.ToDictionary(c => c.Key, c => c.Value)
            };
            return dTOChampion;
        }

        public static Champion ToChampion(this DTOChampion dTOchampion)
        {
            ChampionClass championClass = Enum.TryParse<ChampionClass>(dTOchampion.Class, true, out championClass) ? championClass : ChampionClass.Unknown;
            var champion = new Champion(dTOchampion.Name, championClass, dTOchampion.Icon, dTOchampion.Image, dTOchampion.Bio);
            dTOchampion.Skills.Select(s => champion.AddSkill(s.ToSkill()));
            champion.AddCharacteristics(dTOchampion.Characteristics.Select(c => Tuple.Create(c.Key, c.Value)).ToArray());
            return champion;
        }
    }

}
