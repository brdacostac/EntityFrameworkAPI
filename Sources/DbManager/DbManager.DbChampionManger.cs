using BiblioMilieu;
using BiblioMilieu.Mapper.EnumsMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DbManager
{
    public partial class DbManger : IDataManager
    {
        public class DbChampionManger : DbGeneralManger, IChampionsManager
        {
            public DbChampionManger(DbManger parent) : base(parent) { }

            public async Task<Champion?> AddItem(Champion? item)
            {

                var itemAdded = await parent.DbContext.ChampionsSet.AddAsync(item.ToDb());
                await parent.DbContext.SaveChangesAsync();

                return itemAdded.Entity.ToChampion();
            }

            public async Task<bool> DeleteItem(Champion? item)
            {
                var itemDeleted = parent.DbContext.ChampionsSet.Remove(item.ToDb());
                await parent.DbContext.SaveChangesAsync();
                return parent.DbContext.ChampionsSet.Find(item.Name)!=null;
            }

            public async Task<Champion?> GetItemByName(string name)
            {
                var itemByName = await parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).FirstOrDefaultAsync(item => item.Name == name);
                return itemByName.ToChampion();
            }

            public async Task<IEnumerable<Champion?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                        c => true,
                        index, count,
                        orderingPropertyName, descending).Result.Select(c => c.ToChampion());

            }

            public async Task<IEnumerable<Champion?>> GetItemsByCharacteristic(string charName, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                        c => c.caracteristics.Any(c => c.key.Equals(charName)),
                        index, count,
                        orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public async Task<IEnumerable<Champion?>> GetItemsByClass(ChampionClass championClass, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                         c => c.Class.ToChampionClass().Equals(championClass),
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public async Task<IEnumerable<Champion?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                         c => c.Name.Contains(substring),
                         index, count,
                         orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public async Task<IEnumerable<Champion?>> GetItemsByRunePage(RunePage? runePage, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).Include(c => c.RunePages).GetItemsWithFilterAndOrdering(
                       c => c.RunePages.Any(rp => rp.Equals(runePage.ToDb())),
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public async Task<IEnumerable<Champion?>> GetItemsBySkill(Skill? skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                       c => skill != null && c.Skills.Any(s => s.Name.Equals(skill.Name)),
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public async Task<IEnumerable<Champion?>> GetItemsBySkill(string skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
            {
                return parent.DbContext.ChampionsSet.Include(c => c.Skins).Include(c => c.Skills).Include(c => c.caracteristics).GetItemsWithFilterAndOrdering(
                       c => skill != null && c.Skills.Any(s => s.Name.Equals(skill)),
                       index, count,
                       orderingPropertyName, descending).Result.Select(c => c.ToChampion());
            }

            public Task<int> GetNbItems()
            {
                return parent.DbContext.ChampionsSet.CountAsync();
            }

            public Task<int> GetNbItemsByCharacteristic(string charName)
            {
                return parent.DbContext.ChampionsSet.Where(c => c.Name.Equals(charName)).CountAsync();
            }

            public Task<int> GetNbItemsByClass(ChampionClass championClass)
            {
                return parent.DbContext.ChampionsSet.Where(c => championClass.Equals(c.Class)).CountAsync();
            }

            public Task<int> GetNbItemsByName(string substring)
            {
                return parent.DbContext.ChampionsSet.Where(c => c.Name.Equals(substring)).CountAsync();
            }

            public Task<int> GetNbItemsByRunePage(RunePage? runePage)
            {
                return parent.DbContext.ChampionsSet.Where(c => c.RunePages.Any(r => r.Equals(runePage.ToDb()))).CountAsync();
            }

            public Task<int> GetNbItemsBySkill(Skill? skill)
            {
                return parent.DbContext.ChampionsSet.Where(c => c.Skills.Any(s => s.Equals(skill.ToDb(c)))).CountAsync();
            }

            public Task<int> GetNbItemsBySkill(string skill)
            {
                return parent.DbContext.ChampionsSet.Where(c => c.Skills.Any(s => s.Name.Equals(skill))).CountAsync();
            }

            public async Task<Champion?> UpdateItem(Champion? oldItem, Champion? newItem)
            {
                var itemUpdated = parent.DbContext.ChampionsSet.FirstOrDefault(champ => champ.Name == oldItem.Name);
                itemUpdated = newItem.ToDb();
                parent.DbContext.SaveChanges();
                return itemUpdated.ToChampion();
            }
        }
    }
}
