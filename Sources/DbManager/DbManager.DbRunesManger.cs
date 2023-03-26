using BiblioMilieu;
using BiblioMilieu.Mapper.EnumsMapper;
using Microsoft.EntityFrameworkCore;
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
        public class DbRunesManger : DbGeneralManger, IRunesManager
        {
            public DbRunesManger(DbManger parent) : base(parent) { }

            public async Task<Model.Rune?> AddItem(Model.Rune? item)
            {
                var itemAdded = await parent.DbContext.RunesSet.AddAsync(item.ToDb());
                await parent.DbContext.SaveChangesAsync();

                return itemAdded.Entity?.ToRune();
            }

            public async Task<bool> DeleteItem(Model.Rune? item)
            {
                var itemDeleted = parent.DbContext.RunesSet.Remove(parent.DbContext.RunesSet.FirstOrDefault(i => i.Name == item.Name));
                await parent.DbContext.SaveChangesAsync();
                return parent.DbContext.SkinsSet.Find(item.Name) != null;
            }

            public async Task<Model.Rune?> GetItemByName(string name)
            {
                var itemByName = await parent.DbContext.RunesSet.FirstOrDefaultAsync(item => item.Name == name);
                return itemByName?.ToRune();
            }

            public async Task<IEnumerable<Model.Rune?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunesSet.GetItemsWithFilterAndOrdering(
                       c => true,
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToRune());
            }

            public async Task<IEnumerable<Model.Rune?>> GetItemsByFamily(RuneFamily family, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunesSet.GetItemsWithFilterAndOrdering(
                         c => c.Family.ToRuneFamily().Equals(family),
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToRune());
            }

            public async Task<IEnumerable<Model.Rune?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.RunesSet.GetItemsWithFilterAndOrdering(
                          c => c.Name.Contains(substring),
                          index, count,
                          orderingPropertyName, descending).Result.Select(c => c.ToRune());
            }

            public Task<int> GetNbItems()
            {
                return parent.DbContext.RunesSet.CountAsync();
            }

            public Task<int> GetNbItemsByFamily(RuneFamily family)
            {
                return parent.DbContext.RunesSet.Where(r => r.Family.ToRuneFamily().Equals(family)).CountAsync();
            }

            public Task<int> GetNbItemsByName(string substring)
            {
                return parent.DbContext.RunesSet.Where(c => c.Name.Equals(substring)).CountAsync();
            }

            public async Task<Model.Rune?> UpdateItem(Model.Rune? oldItem, Model.Rune? newItem)
            {
                var itemUpdated = parent.DbContext.RunesSet.Find(oldItem.Name);
                itemUpdated = newItem.ToDb();
                parent.DbContext.SaveChanges();
                return itemUpdated.ToRune();
            }
        }
    }
}
