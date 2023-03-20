using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace TestEntityUT
{
    public class RunePage_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
            .UseInMemoryDatabase(databaseName: "Add_Test_Database_RunePage")
            .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };
     

                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(2, context.RunePagesSet.Count());
                Assert.Equal("Rune Page 1", context.RunePagesSet.First().Name);
            }

        }

        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database_RunePage")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };


                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                // Modifier une donnée
                RunePagesDb runeToUpdate = context.RunePagesSet.First();
                runeToUpdate.Name = "La grande rune";
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                // Vérifier que la donnée a bien été modifiée
                RunePagesDb runeToUpdate = context.RunePagesSet.First();
                Assert.Equal("La grande rune", runeToUpdate.Name);
            }
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Rune Page 2", 1 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Delete_Test(string nameToDelete, int expectedCount)
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database_RunePage")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };

                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);

                context.SaveChanges();

                context.RunePagesSet.Remove(context.RunePagesSet.Where(n => n.Name.ToLower().Contains(nameToDelete.ToLower())).First());
                context.SaveChanges();

                Assert.Equal(expectedCount, context.RunePagesSet.Count());
            }
        }

        [Fact]
        public void GetAllCategory_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllCategory_Test_Database_RunePage")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };


                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var runes = context.RunePagesSet.ToList();

                Assert.Equal(2, runes.Count);

                var rune1 = context.RunePagesSet.FirstOrDefault(c => c.Name == "Rune Page 1");
                Assert.NotNull(rune1);
                Assert.Equal("Rune Page 1", rune1.Name);

                var rune2 = context.RunePagesSet.FirstOrDefault(c => c.Name == "Rune Page 2");
                Assert.NotNull(rune2);
                Assert.Equal("Rune Page 2", rune2.Name);

               
            }
        }

        [Fact]
        public void ErrorCategory_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorCategory_Test_Database_RunePage")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                var runePage1 = new RunePagesDb { Id = 1, Name = null };

                context.RunePagesSet.Add(runePage1);

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }


        }
    }
}
