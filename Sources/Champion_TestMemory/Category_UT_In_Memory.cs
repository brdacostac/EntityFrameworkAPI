using Entity_framework;
using Entity_framework.DataBase;
using Entity_framework.Enums;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace TestEntityUT
{
    public class Category_UT_In_Memory
    {
        [Fact]
        public void Add_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
            .UseInMemoryDatabase(databaseName: "Add_Test_Database_Category")
            .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };
                RuneDB rune1 = new RuneDB { Name = "Rune 1", Description = "Test Rune 1", Icon = "icon1.png", Family = RuneFamilyDb.Precision };
                RuneDB rune2 = new RuneDB { Name = "Rune 2", Description = "Test Rune 2", Icon = "icon2.png", Family = RuneFamilyDb.Domination };
                RuneDB rune3 = new RuneDB { Name = "Rune 3", Description = "Test Rune 3", Icon = "icon3.png", Family = RuneFamilyDb.Unknown };

                CategoryDicDB category1 = new CategoryDicDB { category = CategoryDb.Major, runePage = runePage1, rune = rune1 };
                CategoryDicDB category2 = new CategoryDicDB { category = CategoryDb.Minor1, runePage = runePage1, rune = rune2 };
                CategoryDicDB category3 = new CategoryDicDB { category = CategoryDb.Minor2, runePage = runePage1, rune = rune3 };
                CategoryDicDB category4 = new CategoryDicDB { category = CategoryDb.OtherMinor1, runePage = runePage2, rune = rune1 };


                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);
                context.RunesSet.Add(rune1);
                context.RunesSet.Add(rune2);
                context.RunesSet.Add(rune3);
                context.CategoryRunePageSet.Add(category1);
                context.CategoryRunePageSet.Add(category2);
                context.CategoryRunePageSet.Add(category3);
                context.CategoryRunePageSet.Add(category4);

                context.SaveChanges();
            
                Assert.Equal(2, context.RunePagesSet.Count());
                Assert.Equal(4, context.CategoryRunePageSet.Count());
                Assert.Equal(3, context.RunesSet.Count());
                Assert.Equal(CategoryDb.Major, context.CategoryRunePageSet.First().category);
                Assert.Equal("Rune 1", context.CategoryRunePageSet.Include(x => x.rune).First().rune.Name);
                Assert.Equal("Rune Page 1", context.CategoryRunePageSet.Include(x => x.runePage).First().runePage.Name);

                context.RunePagesSet.Remove(runePage1);
                context.RunePagesSet.Remove(runePage2);
                context.RunesSet.Remove(rune1);
                context.RunesSet.Remove(rune2);
                context.RunesSet.Remove(rune3);

                context.SaveChanges();
            }

        }

        [Fact]
        public void Modify_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Modify_Test_Database_Category")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };
                RuneDB rune1 = new RuneDB { Name = "Rune 1", Description = "Test Rune 1", Icon = "icon1.png", Family = RuneFamilyDb.Precision };
                RuneDB rune2 = new RuneDB { Name = "Rune 2", Description = "Test Rune 2", Icon = "icon2.png", Family = RuneFamilyDb.Domination };
                RuneDB rune3 = new RuneDB { Name = "Rune 3", Description = "Test Rune 3", Icon = "icon3.png", Family = RuneFamilyDb.Unknown };

                CategoryDicDB category1 = new CategoryDicDB { category = CategoryDb.Major, runePage = runePage1, rune = rune1 };
                CategoryDicDB category2 = new CategoryDicDB { category = CategoryDb.Minor1, runePage = runePage1, rune = rune2 };
                CategoryDicDB category3 = new CategoryDicDB { category = CategoryDb.Minor2, runePage = runePage1, rune = rune3 };
                CategoryDicDB category4 = new CategoryDicDB { category = CategoryDb.OtherMinor1, runePage = runePage2, rune = rune1 };


                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);
                context.RunesSet.Add(rune1);
                context.RunesSet.Add(rune2);
                context.RunesSet.Add(rune3);
                context.CategoryRunePageSet.Add(category1);
                context.CategoryRunePageSet.Add(category2);
                context.CategoryRunePageSet.Add(category3);
                context.CategoryRunePageSet.Add(category4);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                // Modifier une donnée
                CategoryDicDB categoryDicToUpdate = context.CategoryRunePageSet.First();
                categoryDicToUpdate.category = CategoryDb.OtherMinor2;
                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {
                // Vérifier que la donnée a bien été modifiée
                CategoryDicDB updatedCategoryDic = context.CategoryRunePageSet.First();
                Assert.Equal(CategoryDb.OtherMinor2, updatedCategoryDic.category);
            }
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Rune Page 1", 1 };
            yield return new object[] { "Rune Page 2", 2 };
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Delete_Test(string nameToDelete, int expectedCount)
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database_Category")
                .Options;

            using (var context = new EntityDbContexte(options))
            {
                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };
                RuneDB rune1 = new RuneDB { Name = "Rune 1", Description = "Test Rune 1", Icon = "icon1.png", Family = RuneFamilyDb.Precision };
                RuneDB rune2 = new RuneDB { Name = "Rune 2", Description = "Test Rune 2", Icon = "icon2.png", Family = RuneFamilyDb.Domination };
                RuneDB rune3 = new RuneDB { Name = "Rune 3", Description = "Test Rune 3", Icon = "icon3.png", Family = RuneFamilyDb.Unknown };
                CategoryDicDB category1 = new CategoryDicDB { category = CategoryDb.Major, runePage = runePage1, rune = rune1 };
                CategoryDicDB category2 = new CategoryDicDB { category = CategoryDb.Minor1, runePage = runePage1, rune = rune2 };
                CategoryDicDB category3 = new CategoryDicDB { category = CategoryDb.Minor2, runePage = runePage1, rune = rune3 };
                CategoryDicDB category4 = new CategoryDicDB { category = CategoryDb.OtherMinor1, runePage = runePage2, rune = rune1 };
                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);
                context.RunesSet.Add(rune1);
                context.RunesSet.Add(rune2);
                context.RunesSet.Add(rune3);
                context.CategoryRunePageSet.Add(category1);
                context.CategoryRunePageSet.Add(category2);
                context.CategoryRunePageSet.Add(category3);
                context.CategoryRunePageSet.Add(category4);
                context.SaveChanges();

                context.RunePagesSet.Remove(context.RunePagesSet.Where(n => n.Name.ToLower().Contains(nameToDelete.ToLower())).First());
                context.SaveChanges();

                Assert.Equal(expectedCount, context.RunePagesSet.Count());


                context.RunesSet.Remove(rune1);
                context.RunesSet.Remove(rune2);
                context.RunesSet.Remove(rune3);
                context.SaveChanges();
            }
        }

        [Fact]
        public void GetAllCategory_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "GetAllCategory_Test_Database_Category")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                RunePagesDb runePage1 = new RunePagesDb { Name = "Rune Page 1" };
                RunePagesDb runePage2 = new RunePagesDb { Name = "Rune Page 2" };
                RuneDB rune1 = new RuneDB { Name = "Rune 1", Description = "Test Rune 1", Icon = "icon1.png", Family = RuneFamilyDb.Precision };
                RuneDB rune2 = new RuneDB { Name = "Rune 2", Description = "Test Rune 2", Icon = "icon2.png", Family = RuneFamilyDb.Domination };
                RuneDB rune3 = new RuneDB { Name = "Rune 3", Description = "Test Rune 3", Icon = "icon3.png", Family = RuneFamilyDb.Unknown };

                CategoryDicDB category1 = new CategoryDicDB { category = CategoryDb.Major, runePage = runePage1, rune = rune1 };
                CategoryDicDB category2 = new CategoryDicDB { category = CategoryDb.Minor1, runePage = runePage1, rune = rune2 };
                CategoryDicDB category3 = new CategoryDicDB { category = CategoryDb.Minor2, runePage = runePage1, rune = rune3 };
                CategoryDicDB category4 = new CategoryDicDB { category = CategoryDb.OtherMinor1, runePage = runePage2, rune = rune1 };


                context.RunePagesSet.Add(runePage1);
                context.RunePagesSet.Add(runePage2);
                context.RunesSet.Add(rune1);
                context.RunesSet.Add(rune2);
                context.RunesSet.Add(rune3);
                context.CategoryRunePageSet.Add(category1);
                context.CategoryRunePageSet.Add(category2);
                context.CategoryRunePageSet.Add(category3);
                context.CategoryRunePageSet.Add(category4);

                context.SaveChanges();
            }

            using (var context = new EntityDbContexte(options))
            {

                var categorys = context.CategoryRunePageSet.ToList();

                Assert.Equal(4, categorys.Count);

                var category1 = context.CategoryRunePageSet.Include(x => x.rune).FirstOrDefault(c => c.category == CategoryDb.Major);
                Assert.NotNull(category1);
                Assert.Equal("Test Rune 1", category1.rune.Description);

                var category2 = context.CategoryRunePageSet.Include(x => x.rune).FirstOrDefault(c => c.category == CategoryDb.Minor1);
                Assert.NotNull(category2);
                Assert.Equal("Test Rune 2", category2.rune.Description);

                var category3 = context.CategoryRunePageSet.Include(x => x.rune).FirstOrDefault(c => c.category == CategoryDb.Minor2);
                Assert.NotNull(category3);
                Assert.Equal("Test Rune 3", category3.rune.Description);

                var category4 = context.CategoryRunePageSet.Include(x => x.rune).FirstOrDefault(c => c.category == CategoryDb.OtherMinor1);
                Assert.NotNull(category4);
                Assert.Equal("Test Rune 1", category4.rune.Description);
            }
        }

        [Fact]
        public void ErrorCategory_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "ErrorCategory_Test_Database_Category")
                .Options;

            using (var context = new EntityDbContexte(options))
            {

                var rune1 = new RuneDB { Id = 1, Name = "Test Rune" };

                context.CategoryRunePageSet.Add(new CategoryDicDB { Id = 1, rune = rune1 });

                Assert.Throws<DbUpdateException>(() => context.SaveChanges());
            }


        }
    }
}
