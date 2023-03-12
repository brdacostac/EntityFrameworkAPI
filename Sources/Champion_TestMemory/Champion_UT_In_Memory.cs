using Entity_framework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Champion_TestMemory
{
    public class Champion_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database")
                .Options;


            using (var context = new EntityDbContexte(options))
            {

                ChampionDB Gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB Vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB Corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(Gnar);
                context.ChampionsSet.Add(Vladimir);
                context.ChampionsSet.Add(Corki);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(3, context.ChampionsSet.Count());
                Assert.Equal("Gnar", context.ChampionsSet.First().Name);
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

                ChampionDB gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(gnar);
                context.ChampionsSet.Add(vladimir);
                context.ChampionsSet.Add(corki);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "nar";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "vlad";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var vladimir = context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                vladimir.Name = "Annie";
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "nar";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "annie";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
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

                ChampionDB gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(gnar);
                context.ChampionsSet.Add(vladimir);
                context.ChampionsSet.Add(corki);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "nar";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "vlad";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                context.ChampionsSet.Remove(context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "vlad";
                Assert.NotEqual(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }

        /*
        [Fact]
        public void GetChampion_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetChampion_Test_Database")
                .Options;

            using (var context = new ChampionsDbContexte(options))
            {
                ChampionController championService = new ChampionController(context);

                var gnar = championService.GetChampion("Gnar");
                var vladimir = championService.GetChampion("Vladimir");
                var corki = championService.GetChampion("Corki");

                Assert.NotNull(gnar);
                Assert.NotNull(vladimir);
                Assert.NotNull(corki);

                Assert.Equal("Gnar", gnar.Name);
                Assert.Equal("Vladimir", vladimir.Name);
                Assert.Equal("Corki", corki.Name);
            }
        }
        */
    }
}