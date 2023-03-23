using DTOLol;
using Model;

namespace MapperApi.Mapper
{
    public static class SkillMapper
    {
        public static DTOSkill ToDto(this Skill skill)
        {
            return new DTOSkill
            {
                Name = skill.Name,
                Type = skill.Type.ToString(),
                Description = skill.Description
            };
        }

        public static Skill ToSkill(this DTOSkill skillDto)
        {
            return new Skill(skillDto.Name,Enum.Parse<SkillType>(skillDto.Type),description: skillDto.Description);
        }
    }
}
