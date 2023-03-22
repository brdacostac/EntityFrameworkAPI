using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;


namespace TestEntityUT
{
    public class Caracteristic_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Add_Test_Database_Caracteristic")
                .Options;


            using (var context = new EntityDbContexte(options))
            {
                var champion = new ChampionDB { Name = "Ahri", Class = ClassChampionDb.Unknown, Bio = "Test Ahri Bio", Icon = "ahri-icon.png" };

                var caracteristic1 = new CaracteristicDb { Id = 1, key = "Health", valeur = 500, champion = champion };
                var caracteristic2 = new CaracteristicDb { Id = 2, key = "Mana", valeur = 200, champion = champion };

                context.CaracteristicSet.Add(caracteristic1);
                context.CaracteristicSet.Add(caracteristic2);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                Assert.Equal(2, context.CaracteristicSet.Count());
                Assert.Equal("Health", context.CaracteristicSet.First().key);
            }
        }

        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database_Caracteristic")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                var champion = new ChampionDB { Name = "Ahri", Class = ClassChampionDb.Unknown, Bio = "Test Ahri Bio", Icon = "ahri-icon.png" };
                var caracteristic1 = new CaracteristicDb { Id = 1, key = "Health", valeur = 500, champion = champion, ChampionForeignKey = 1 };
                var caracteristic2 = new CaracteristicDb { Id = 2, key = "Mana", valeur = 200, champion = champion, ChampionForeignKey = 1 };

                context.Add(caracteristic1);
                context.Add(caracteristic2);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string keyToFind = "health";
                Assert.Equal(1, context.CaracteristicSet.Where(c => c.key.ToLower().Contains(keyToFind)).Count());

                var caracteristic = context.CaracteristicSet.Where(c => c.key.ToLower().Contains(keyToFind)).First();
                caracteristic.valeur = 550;
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                string keyToFind = "health";
                Assert.Equal(1, context.CaracteristicSet.Where(c => c.key.ToLower().Contains(keyToFind)).Count());
                var caracteristic = context.CaracteristicSet.Where(c => c.key.ToLower().Contains(keyToFind)).First();
                Assert.Equal(550, caracteristic.valeur);
            }
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Health", 0 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Delete_Test(string nameToDelete, int expectedCount)
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database_Caracteristic")
                .Options;


            using (var context = new EntityDbContexte(options))
            {
                var champion = new ChampionDB { Name = "Ahri", Class = ClassChampionDb.Unknown, Bio = "Test Ahri Bio", Icon = "ahri-icon.png" };

                var caracteristic1 = new CaracteristicDb { Id = 1, key = "Health", valeur = 500, champion = champion, ChampionForeignKey = 1 };

                context.CaracteristicSet.Add(caracteristic1);
                context.SaveChanges();
            

                context.CaracteristicSet.Remove(context.CaracteristicSet.Where(n => n.key.ToLower().Contains(nameToDelete.ToLower())).First());
                context.SaveChanges();

                Assert.Equal(expectedCount, context.CaracteristicSet.Count());
            }

        }

        [Fact]
        public void GetAllRune_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllRune_Test_Caracteristic")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                var champion = new ChampionDB { Name = "Ahri", Class = ClassChampionDb.Unknown, Bio = "Test Ahri Bio", Icon = "ahri-icon.png" };
                var caracteristic1 = new CaracteristicDb { Id = 1, key = "Health", valeur = 500, champion = champion, ChampionForeignKey = 1 };
                var caracteristic2 = new CaracteristicDb { Id = 2, key = "Mana", valeur = 200, champion = champion, ChampionForeignKey = 1 };


                context.Add(caracteristic1);
                context.Add(caracteristic2);
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var caracteristics = context.CaracteristicSet.ToList();

                Assert.Equal(2, caracteristics.Count);

                var health = caracteristics.FirstOrDefault(c => c.key == "Health");
                Assert.NotNull(health);
                Assert.Equal(500, health.valeur);

                var mana = caracteristics.FirstOrDefault(c => c.key == "Mana");
                Assert.NotNull(mana);
                Assert.Equal(200, mana.valeur);

            }
        }
    }
}
