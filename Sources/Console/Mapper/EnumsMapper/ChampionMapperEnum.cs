using Entity_framework;
using Model;

namespace BiblioMilieu.Mapper.EnumsMapper
{
    public static class ChampionMapperEnum
    {
        public static ClassChampionDb ToDb(this ChampionClass championClass)
        {
            switch (championClass)
            {
                case ChampionClass.Assassin:
                    return ClassChampionDb.Assassin;
                case ChampionClass.Fighter:
                    return ClassChampionDb.Fighter;
                case ChampionClass.Mage:
                    return ClassChampionDb.Mage;
                case ChampionClass.Marksman:
                    return ClassChampionDb.Marksman;
                case ChampionClass.Support:
                    return ClassChampionDb.Support;
                case ChampionClass.Tank:
                    return ClassChampionDb.Tank;
                default:
                    return ClassChampionDb.Unknown;
            }
                   
        }

        public static ChampionClass ToChampionClass(this ClassChampionDb classChampionDb)
        {
            switch (classChampionDb)
            {
                case ClassChampionDb.Assassin:
                    return ChampionClass.Assassin;
                case ClassChampionDb.Fighter:
                    return ChampionClass.Fighter;
                case ClassChampionDb.Mage:
                    return ChampionClass.Mage;
                case ClassChampionDb.Marksman:
                    return ChampionClass.Marksman;
                case ClassChampionDb.Support:
                    return ChampionClass.Support;
                case ClassChampionDb.Tank:
                    return ChampionClass.Tank;
                default:
                    return ChampionClass.Unknown;
            }

        }

    }
}
