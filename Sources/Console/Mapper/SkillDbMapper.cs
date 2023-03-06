using Model;
using Entity_framework;
using BiblioMilieu.Mapper.EnumsMapper;

namespace BiblioMilieu
{
    public static class SkillDbMapper
    {
        public static SkillDB ToDb(this Skill skill)
        {
            SkillDB skillDb = new SkillDB()
            {
                Name = skill.Name,
                Description = skill.Description,
                Type = skill.Type.ToDb(),
            };
            return skillDb;
        }

        public static Skill ToSkill(this SkillDB skillDb)
        {
            return new Skill(skillDb.Name, skillDb.Type.ToSkillType(), skillDb.Description);
        }

    }
}
