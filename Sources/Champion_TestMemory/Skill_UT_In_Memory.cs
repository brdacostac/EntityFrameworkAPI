using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace TestEntityUT
{
    public class Skill_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database")
                .Options;


            using (var context = new EntityDbContexte(options))
            {

                SkillDB Passive = new SkillDB { Name = "Frost Shot", Type= SkillTypeSkillDb.Passive, Description = "Teste Passive"};
                SkillDB FirstSkill = new SkillDB { Name = "Rangers Focus", Type = SkillTypeSkillDb.Basic, Description = "Teste First Skill" };
                SkillDB SecondSkill = new SkillDB { Name = "Volley", Type = SkillTypeSkillDb.Basic, Description = "Teste Second Skill " };
                SkillDB ThirdSkill = new SkillDB { Name = "HawkShot", Type = SkillTypeSkillDb.Basic, Description = "Teste Third Skill" };
                SkillDB Ultimate = new SkillDB { Name = "Echanted Crystal Arrow", Type = SkillTypeSkillDb.Ultimate, Description = "Teste Ultimate" };

                context.SkillSet.Add(Passive);
                context.SkillSet.Add(FirstSkill);
                context.SkillSet.Add(SecondSkill);
                context.SkillSet.Add(ThirdSkill);
                context.SkillSet.Add(Ultimate);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(5, context.SkillSet.Count());
                Assert.Equal("Frost Shot", context.SkillSet.First().Name);
            }
        }

        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                SkillDB Passive = new SkillDB { Name = "Frost Shot", Type = SkillTypeSkillDb.Passive, Description = "Teste Passive" };
                SkillDB FirstSkill = new SkillDB { Name = "Rangers Focus", Type = SkillTypeSkillDb.Basic, Description = "Teste First Skill" };
                SkillDB SecondSkill = new SkillDB { Name = "Volley", Type = SkillTypeSkillDb.Basic, Description = "Teste Second Skill " };
                SkillDB ThirdSkill = new SkillDB { Name = "HawkShot", Type = SkillTypeSkillDb.Basic, Description = "Teste Third Skill" };
                SkillDB Ultimate = new SkillDB { Name = "Echanted Crystal Arrow", Type = SkillTypeSkillDb.Ultimate, Description = "Teste Ultimate" };

                context.SkillSet.Add(Passive);
                context.SkillSet.Add(FirstSkill);
                context.SkillSet.Add(SecondSkill);
                context.SkillSet.Add(ThirdSkill);
                context.SkillSet.Add(Ultimate);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "rost";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "frost shot";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var frostShot = context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                frostShot.Name = "Ice Arrow";
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "shot";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "ice arrow";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }
        }

        [Fact]
        public void Delete_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                SkillDB Passive = new SkillDB { Name = "Frost Shot", Type = SkillTypeSkillDb.Passive, Description = "Teste Passive" };
                SkillDB FirstSkill = new SkillDB { Name = "Rangers Focus", Type = SkillTypeSkillDb.Basic, Description = "Teste First Skill" };
                SkillDB SecondSkill = new SkillDB { Name = "Volley", Type = SkillTypeSkillDb.Basic, Description = "Teste Second Skill " };
                SkillDB ThirdSkill = new SkillDB { Name = "HawkShot", Type = SkillTypeSkillDb.Basic, Description = "Teste Third Skill" };
                SkillDB Ultimate = new SkillDB { Name = "Echanted Crystal Arrow", Type = SkillTypeSkillDb.Ultimate, Description = "Teste Ultimate" };

                context.SkillSet.Add(Passive);
                context.SkillSet.Add(FirstSkill);
                context.SkillSet.Add(SecondSkill);
                context.SkillSet.Add(ThirdSkill);
                context.SkillSet.Add(Ultimate);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "rost";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "volley";
                Assert.Equal(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());


                context.SkillSet.Remove(context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "volley";
                Assert.NotEqual(1, context.SkillSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }

        [Fact]
        public void GetAllSkill_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllSkill_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                SkillDB Passive = new SkillDB { Name = "Frost Shot", Type = SkillTypeSkillDb.Passive, Description = "Teste Passive" };
                SkillDB FirstSkill = new SkillDB { Name = "Rangers Focus", Type = SkillTypeSkillDb.Basic, Description = "Teste First Skill" };
                SkillDB SecondSkill = new SkillDB { Name = "Volley", Type = SkillTypeSkillDb.Basic, Description = "Teste Second Skill" };
                SkillDB ThirdSkill = new SkillDB { Name = "HawkShot", Type = SkillTypeSkillDb.Basic, Description = "Teste Third Skill" };
                SkillDB Ultimate = new SkillDB { Name = "Echanted Crystal Arrow", Type = SkillTypeSkillDb.Ultimate, Description = "Teste Ultimate" };

                context.SkillSet.Add(Passive);
                context.SkillSet.Add(FirstSkill);
                context.SkillSet.Add(SecondSkill);
                context.SkillSet.Add(ThirdSkill);
                context.SkillSet.Add(Ultimate);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var skills = context.SkillSet.ToList();

                Assert.Equal(5, skills.Count);

                var passive = skills.FirstOrDefault(c => c.Name == "Frost Shot");
                Assert.NotNull(passive);
                Assert.Equal("Teste Passive", passive.Description);

                var firstSkill  = skills.FirstOrDefault(c => c.Name == "Rangers Focus");
                Assert.NotNull(firstSkill);
                Assert.Equal("Teste First Skill", firstSkill.Description);

                var secondSkill = skills.FirstOrDefault(c => c.Name == "Volley");
                Assert.NotNull(secondSkill);
                Assert.Equal("Teste Second Skill", secondSkill.Description);

                var thirdSkill = skills.FirstOrDefault(c => c.Name == "HawkShot");
                Assert.NotNull(thirdSkill);
                Assert.Equal("Teste Third Skill", thirdSkill.Description);

                var ultimate = skills.FirstOrDefault(c => c.Name == "Echanted Crystal Arrow");
                Assert.NotNull(ultimate);
                Assert.Equal("Teste Ultimate", ultimate.Description);


            }
        }

        [Fact]
        public void ErrorSkill_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorSkill_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                context.SkillSet.Add(new SkillDB { Name = null, Description = "Test", Type = SkillTypeSkillDb.Unknown });

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }
        }
    }
}
