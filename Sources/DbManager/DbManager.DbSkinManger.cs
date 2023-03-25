using BiblioMilieu;
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
        public class DbSkinManger : DbGeneralManger, ISkinsManager
        {
            public DbSkinManger(DbManger parent) : base(parent) { }

            public async Task<Skin?> AddItem(Skin? item)
            {
                var champ = parent.DbContext.ChampionsSet.FirstOrDefault(champion => champion.Name==item.Name);
              
                var itemAdded = await parent.DbContext.SkinsSet.AddAsync(item.ToDb(champ));
                await parent.DbContext.SaveChangesAsync();

                return itemAdded.Entity?.ToSkin();
            }

            public async Task<bool> DeleteItem(Skin? item)
            {
                var itemDeleted = parent.DbContext.SkinsSet.Remove(item.ToDb());
                await parent.DbContext.SaveChangesAsync();
                return parent.DbContext.SkinsSet.Find(item.Name) != null;
            }

            public async Task<Skin?> GetItemByName(string name)
            {
                var itemByName = await parent.DbContext.SkinsSet.Include(c => c.Champion).FirstOrDefaultAsync(item => item.Name == name);
                return itemByName?.ToSkin();
            }

            public async Task<IEnumerable<Skin?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.SkinsSet.Include(c => c.Champion).GetItemsWithFilterAndOrdering(
                       c => true,
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToSkin());
            }

            public async Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.SkinsSet.Include(c => c.Champion).GetItemsWithFilterAndOrdering(
                       c =>  c.Champion.Name.Contains(champion.Name),
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToSkin());
            }

            public async Task<IEnumerable<Skin?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.SkinsSet.Include(c => c.Champion).GetItemsWithFilterAndOrdering(
                         c => c.Name.Contains(substring),
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToSkin());
            }

            public Task<int> GetNbItems()
            {
                return parent.DbContext.SkinsSet.CountAsync();
            }

            public Task<int> GetNbItemsByChampion(Champion? champion)
            {
                return parent.DbContext.SkinsSet.Where(c => c.Champion.Name.Contains(champion.Name)).CountAsync();
            }

            public Task<int> GetNbItemsByName(string substring)
            {
                return parent.DbContext.SkinsSet.Where(c => c.Name.Equals(substring)).CountAsync();
            }

            public async Task<Skin?> UpdateItem(Skin? oldItem, Skin? newItem)
            {
                var itemUpdated = parent.DbContext.SkinsSet.FirstOrDefault(e => e.Name==oldItem.Name);
                itemUpdated = newItem.ToDb();
                parent.DbContext.SaveChanges();
                return itemUpdated?.ToSkin();
            }
        }
    }
}
