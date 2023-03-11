using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace TestEntityUT
{
    public class Rune_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<ChampionsDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database")
                .Options;


            using (var context = new ChampionsDbContexte(options))
            {

                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family=(new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = (new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = (new RuneFamilyDb()), Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                Assert.Equal(3, context.RunesSet.Count());
                Assert.Equal("Glacial Augment", context.RunesSet.First().Name);
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

                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = (new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = (new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = (new RuneFamilyDb()), Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "lacial";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "guardian";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var guardian = context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                guardian.Name = "Perfect Timing";
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "lacial";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "perfect timing";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
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

                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = (new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = (new RuneFamilyDb()), Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = (new RuneFamilyDb()), Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "lacial";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "guardian";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                context.RunesSet.Remove(context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new ChampionsDbContexte(options))
            {
                string nameToFind = "guaridan";
                Assert.NotEqual(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }
    }
}
