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
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database")
                .Options;


            using (var context = new ChampionsDbContexte(options))
            {

                ChampionDB Gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB Vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB Corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(Gnar);
                context.ChampionsSet.Add(Vladimir);
                context.ChampionsSet.Add(Corki);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                Assert.Equal(3, context.ChampionsSet.Count());
                Assert.Equal("Gnar", context.ChampionsSet.First().Name);
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

                ChampionDB gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(gnar);
                context.ChampionsSet.Add(vladimir);
                context.ChampionsSet.Add(corki);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "nar";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "vlad";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var vladimir = context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                vladimir.Name = "Annie";
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
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
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database")
                .Options;

            using (var context = new ChampionsDbContexte(options))
            {

                ChampionDB gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(gnar);
                context.ChampionsSet.Add(vladimir);
                context.ChampionsSet.Add(corki);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "nar";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "vlad";
                Assert.Equal(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                context.ChampionsSet.Remove(context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "vlad";
                Assert.NotEqual(1, context.ChampionsSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }

        
        [Fact]
        public void GetAllChampion_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllChampion_Test_Database")
                .Options;

            using (var context = new ChampionsDbContexte(options))
            {

                ChampionDB gnar = new ChampionDB { Name = "Gnar", Bio = "Teste gnar", Icon = "dzadaz" };
                ChampionDB vladimir = new ChampionDB { Name = "Vladimir", Bio = "Teste Vlad", Icon = "dzadaz" };
                ChampionDB corki = new ChampionDB { Name = "Corki", Bio = "Teste Corki", Icon = "dzadaz" };

                context.ChampionsSet.Add(gnar);
                context.ChampionsSet.Add(vladimir);
                context.ChampionsSet.Add(corki);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                
                var champions = context.ChampionsSet.ToList();

                Assert.Equal(3, champions.Count);

                var gnar = champions.FirstOrDefault(c => c.Name == "Gnar");
                Assert.NotNull(gnar);
                Assert.Equal("Teste gnar", gnar.Bio);

                var vladimir = champions.FirstOrDefault(c => c.Name == "Vladimir");
                Assert.NotNull(vladimir);
                Assert.Equal("Teste Vlad", vladimir.Bio);

                var corki = champions.FirstOrDefault(c => c.Name == "Corki");
                Assert.NotNull(corki);
                Assert.Equal("Teste Corki", corki.Bio);
            }
        }

        [Fact]
        public void ErrorChampion_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorChampion_Test_Database")
                .Options;

            using (var context = new ChampionsDbContexte(options))
            {

                context.ChampionsSet.Add(new ChampionDB { Name = null, Bio = "Test", Icon = "Test" });

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }

     
        }

    }
}