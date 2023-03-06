using Entity_framework.Enums;
using Model;

namespace BiblioMilieu.Mapper.EnumsMapper
{
    public static class SkillMapperEnum
    {
        public static SkillTypeSkillDb ToDb(this SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Basic:
                    return SkillTypeSkillDb.Basic;
                case SkillType.Passive:
                    return SkillTypeSkillDb.Passive;
                case SkillType.Ultimate:
                    return SkillTypeSkillDb.Ultimate;
                default:
                    return SkillTypeSkillDb.Unknown;
            }
        }

        public static SkillType ToSkillType(this SkillTypeSkillDb skillTypeDb)
        {
            switch (skillTypeDb)
            {
                case SkillTypeSkillDb.Basic:
                    return SkillType.Basic;
                case SkillTypeSkillDb.Passive:
                    return SkillType.Passive;
                case SkillTypeSkillDb.Ultimate:
                    return SkillType.Ultimate;
                default:
                    return SkillType.Unknown;
            }

        }
    }
}
