using Entity_framework.Enums;
using Model;

namespace BiblioMilieu.Mapper.EnumsMapper
{
    public static class RuneFamilyMapperEnum
    {
        public static RuneFamilyDb ToDb(this RuneFamily runeFamily)
        {
            switch (runeFamily)
            {
                case RuneFamily.Domination:
                    return RuneFamilyDb.Domination;
                case RuneFamily.Precision:
                    return RuneFamilyDb.Precision;       
                default:
                    return RuneFamilyDb.Unknown;
            }
        }
        public static RuneFamily ToRuneFamily(this RuneFamilyDb runeFamily)
        {
            switch (runeFamily)
            {
                case RuneFamilyDb.Domination:
                    return RuneFamily.Domination;
                case RuneFamilyDb.Precision:
                    return RuneFamily.Precision;
                default:
                    return RuneFamily.Unknown;
            }
        }
    }
}
