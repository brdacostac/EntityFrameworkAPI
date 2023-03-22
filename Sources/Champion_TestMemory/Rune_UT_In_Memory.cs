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
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database_Rune")
                .Options;


            using (var context = new EntityDbContexte(options))
            {
                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = RuneFamilyDb.Unknown, Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = RuneFamilyDb.Domination, Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = RuneFamilyDb.Precision, Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(3, context.RunesSet.Count());
                Assert.Equal("Glacial Augment", context.RunesSet.First().Name);
            }
        }

        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database_Rune")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = RuneFamilyDb.Unknown, Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = RuneFamilyDb.Domination, Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = RuneFamilyDb.Precision, Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "lacial";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "guardian";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                var guardian = context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First();
                guardian.Name = "Perfect Timing";
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
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
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database_Rune")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = RuneFamilyDb.Unknown, Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = RuneFamilyDb.Domination, Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = RuneFamilyDb.Precision, Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "lacial";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                nameToFind = "guardian";
                Assert.Equal(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());

                context.RunesSet.Remove(context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "guaridan";
                Assert.NotEqual(1, context.RunesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }

        }

        [Fact]
        public void GetAllRune_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllRune_Test_Database_Rune")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                RuneDB Glacial = new RuneDB { Name = "Glacial Augment", Description = "Teste Glacial Augment", Family = RuneFamilyDb.Unknown, Icon = "dzadaz" };
                RuneDB Guardian = new RuneDB { Name = "Guardian", Description = "Teste Guardian", Family = RuneFamilyDb.Domination, Icon = "dzadaz" };
                RuneDB Strike = new RuneDB { Name = "First Strike", Description = "Teste First Strike", Family = RuneFamilyDb.Precision, Icon = "dzadaz" };

                context.RunesSet.Add(Glacial);
                context.RunesSet.Add(Guardian);
                context.RunesSet.Add(Strike);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var runes = context.RunesSet.ToList();

                Assert.Equal(3, runes.Count);

                var glacial = runes.FirstOrDefault(c => c.Name == "Glacial Augment");
                Assert.NotNull(glacial);
                Assert.Equal("Teste Glacial Augment", glacial.Description);

                var guardian = runes.FirstOrDefault(c => c.Name == "Guardian");
                Assert.NotNull(guardian);
                Assert.Equal("Teste Guardian", guardian.Description);

                var strike = runes.FirstOrDefault(c => c.Name == "First Strike");
                Assert.NotNull(strike);
                Assert.Equal("Teste First Strike", strike.Description);

            }
        }

        [Fact]
        public void ErrorRune_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorRune_Test_Database_Rune")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                context.RunesSet.Add(new RuneDB { Name = null, Description = "Test", Family = RuneFamilyDb.Unknown, Icon = null });

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }
        }
    }
}