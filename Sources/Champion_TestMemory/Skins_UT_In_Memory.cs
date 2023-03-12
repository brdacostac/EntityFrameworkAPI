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
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
            .UseInMemoryDatabase(databaseName: "Add_Test_Database")
            .Options;

            using (var context = new EntityDbContexte(options))
            {
                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity", Icon = "Icon Lux" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion, Description = "Description skin1", Icon = "Icon skin1" };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion, Description = "Description skin2", Icon = "Icon skin2" };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion, Description = "Description skin3", Icon = "Icon skin3" };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }
            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(3, context.SkinsSet.Count());
                Assert.Equal("Classic", context.SkinsSet.First().Name);
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
                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity", Icon = "Icon Lux" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion, Description  = "Description skin1", Icon = "Icon skin1" };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion, Description = "Description skin2", Icon = "Icon skin2" };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion, Description = "Description skin3", Icon = "Icon skin3" };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "elementalist";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var elementalist = context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                elementalist.Price = 1500;
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "elementalist";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                var elementalist = context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                Assert.Equal(1500, elementalist.Price);
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

                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity", Icon = "Icon Lux" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion, Description = "Description skin1", Icon = "Icon skin1" };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion, Description = "Description skin2", Icon = "Icon skin2" };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion, Description = "Description skin3", Icon = "Icon skin3" };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "gion";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "classic";
                Assert.Equal(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());


                context.SkinsSet.Remove(context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "classic";
                Assert.NotEqual(1, context.SkinsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }


        [Fact]
        public void GetAllSkin_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllSkin_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                ChampionDB champion = new ChampionDB { Name = "Lux", Bio = "The Lady of Luminosity", Icon = "Icon Lux" };
                context.ChampionsSet.Add(champion);

                SkinDB skin1 = new SkinDB { Name = "Classic", Price = 0, Champion = champion, Description = "Description skin1", Icon = "Icon skin1" };
                SkinDB skin2 = new SkinDB { Name = "Elementalist", Price = 3250, Champion = champion, Description = "Description skin2", Icon = "Icon skin2" };
                SkinDB skin3 = new SkinDB { Name = "Steel Legion", Price = 975, Champion = champion, Description = "Description skin3", Icon = "Icon skin3" };

                context.SkinsSet.Add(skin1);
                context.SkinsSet.Add(skin2);
                context.SkinsSet.Add(skin3);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var skins = context.SkinsSet.ToList();

                Assert.Equal(3, skins.Count);

                var classic = skins.FirstOrDefault(c => c.Name == "Classic");
                Assert.NotNull(classic);
                Assert.Equal("Description skin1", classic.Description);

                var elementalist = skins.FirstOrDefault(c => c.Name == "Elementalist");
                Assert.NotNull(elementalist);
                Assert.Equal("Description skin2", elementalist.Description);

                var steellegion = skins.FirstOrDefault(c => c.Name == "Steel Legion");
                Assert.NotNull(steellegion);
                Assert.Equal("Description skin3", steellegion.Description);


            }
        }

        [Fact]
        public void ErrorSkin_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorSkin_Test_Database")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                context.SkinsSet.Add(new SkinDB { Name = null, Price = 0, Champion = null, Description = "Description skin1", Icon = "Icon skin1" });

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }
        }

    }

}