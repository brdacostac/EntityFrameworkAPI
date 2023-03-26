using BiblioMilieu;
using BiblioMilieu.Mapper.EnumsMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public partial class DbManger : IDataManager
    {
        public class DbRunePagesManger : DbGeneralManger, IRunePagesManager
        {
            public DbRunePagesManger(DbManger parent) : base(parent) { }

            public async Task<RunePage?> AddItem(RunePage? item)
            {
                var itemAdded = await parent.DbContext.RunePagesSet.AddAsync(item.ToDb());
                await parent.DbContext.SaveChangesAsync();

                return itemAdded.Entity?.ToRunePage();
            }

            public async Task<bool> DeleteItem(RunePage? item)
            {
                var itemDeleted = parent.DbContext.RunePagesSet.Remove(parent.DbContext.RunePagesSet.FirstOrDefault(i => i.Name == item.Name));
                await parent.DbContext.SaveChangesAsync();
                return parent.DbContext.RunePagesSet.Find(item.Name) != null;
            }

            public async Task<RunePage?> GetItemByName(string name)
            {
                var itemByName = await parent.DbContext.RunePagesSet.Include(rp => rp.CategoryRunePages).Include(rp => rp.champions).FirstOrDefaultAsync(item => item.Name == name);
                return itemByName?.ToRunePage();
            }

            public async Task<IEnumerable<RunePage?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunePagesSet.Include(rp => rp.CategoryRunePages).Include(rp => rp.champions).GetItemsWithFilterAndOrdering(
                        c => true,
                        index, count,
                        orderingPropertyName, descending).Result.Select(c => c.ToRunePage());
            }

            public async Task<IEnumerable<RunePage?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunePagesSet.Include(rp => rp.CategoryRunePages).Include(rp => rp.champions).GetItemsWithFilterAndOrdering(
                         rp => rp.champions.FirstOrDefault(c => c.Name== champion.Name)!=null,
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToRunePage());
            }

            public async Task<IEnumerable<RunePage?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunePagesSet.Include(rp => rp.CategoryRunePages).Include(rp => rp.champions).GetItemsWithFilterAndOrdering(
                         c => c.Name.Contains(substring),
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToRunePage());
            }

            public async Task<IEnumerable<RunePage?>> GetItemsByRune(Model.Rune? rune, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunePagesSet.Include(rp => rp.CategoryRunePages).Include(rp => rp.champions).GetItemsWithFilterAndOrdering(
                         rp => rp.CategoryRunePages.FirstOrDefault(c => c.rune.Name == rune.Name) != null,
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToRunePage());
            }

            public  Task<int> GetNbItems()
            {
                return parent.DbContext.RunePagesSet.CountAsync();
            }

            public  Task<int> GetNbItemsByChampion(Champion? champion)
            {
               return parent.DbContext.RunePagesSet.Where(rp => rp.champions.FirstOrDefault(c => c.Name == champion.Name)!=null ).CountAsync();
                       
            }

            public  Task<int> GetNbItemsByName(string substring)
            {
                return parent.DbContext.RunePagesSet.Where(c => c.Name== substring).CountAsync();
            }

            public Task<int> GetNbItemsByRune(Model.Rune? rune)
            {
                return parent.DbContext.RunePagesSet.Where(rp => rp.CategoryRunePages.FirstOrDefault(c => c.rune.Name == rune.Name) != null).CountAsync();
            }

            public async Task<RunePage?> UpdateItem(RunePage? oldItem, RunePage? newItem)
            {
                var itemUpdated = parent.DbContext.RunePagesSet.FirstOrDefault(champ => champ.Name == oldItem.Name);
                var newEntity = newItem.ToDb();
                itemUpdated.Name = newEntity.Name;
                parent.DbContext.SaveChanges();
                return itemUpdated?.ToRunePage();
            }
        }
    }
}
