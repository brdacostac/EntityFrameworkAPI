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
            .UseInMemoryDatabase(databaseName: "Add_Test_Database")
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
                Assert.Equal(2, context.RunePagesSet.Count());
                Assert.Equal(4, context.CategoryRunePageSet.Count());
                Assert.Equal(3, context.RunesSet.Count());
                Assert.Equal(CategoryDb.Major, context.CategoryRunePageSet.First().category);
                Assert.Equal("Rune 1", context.CategoryRunePageSet.Include(x => x.rune).First().rune.Name);
                Assert.Equal("Rune Page 1", context.CategoryRunePageSet.Include(x => x.runePage).First().runePage.Name);
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

        [Fact]
        public void Delete_Test()
        {
            var options = new DbContextOptionsBuilder<EntityDbContexte>()
                .UseInMemoryDatabase(databaseName: "Delete_Test_Database")
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
                string nameToFind = "Rune Page 1";
                Assert.Equal(1, context.RunePagesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
                context.RunePagesSet.Remove(context.RunePagesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).First());
                context.SaveChanges();

            }

            using (var context = new EntityDbContexte(options))
            {
                string nameToFind = "Rune Page 1";
                Assert.NotEqual(1, context.RunePagesSet.Where(n => n.Name.ToLower().Contains(nameToFind)).Count());
            }
        }
    }
}
