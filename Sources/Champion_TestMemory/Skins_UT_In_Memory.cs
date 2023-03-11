using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace TestEntityUT
{
    public class Skins_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
            .UseInMemoryDatabase(databaseName: "Add_Test_Database")
            .Options;

            using (var context = new ChampionsDbContexte(options))
            {
                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }
            using (var context = new ChampionsDbContexte(options))
            {
                Assert.Equal(3, context.SkinsSet.Count());
                Assert.Equal("Classic", context.SkinsSet.First().Name);
            }

        }
        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database")
                .Options;

            using (var context = new ChampionsDbContexte(options))
            {
                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "elementalist";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var elementalist = context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                elementalist.Price = 1500;
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "elementalist";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                var elementalist = context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                Assert.Equal(1500, elementalist.Price);
            }
        }

    }

}
