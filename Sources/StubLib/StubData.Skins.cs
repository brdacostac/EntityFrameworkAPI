using System;
using Model;

namespace StubLib
{
	public partial class StubData
	{
		private readonly List<Skin> skins = new();

		private void InitSkins()
		{
			skins.Add(new Skin("Stinger", champions[0]));
			skins.Add(new Skin("Infernal", champions[0]));
			skins.Add(new Skin("All-Star", champions[0]));
			skins.Add(new Skin("Justicar", champions[1]));
			skins.Add(new Skin("Mecha", champions[1]));
			skins.Add(new Skin("Sea Hunter", champions[1]));
			skins.Add(new Skin("Dynasty", champions[2]));
			skins.Add(new Skin("Midnight", champions[2]));
			skins.Add(new Skin("Foxfire", champions[2]));
			skins.Add(new Skin("Cyber Pop", champions[3]));
			skins.Add(new Skin("Crystal Rose", champions[3]));
			skins.Add(new Skin("Elderwood", champions[4]));
			skins.Add(new Skin("Snow Day", champions[4]));
			skins.Add(new Skin("Bard", champions[4]));
			skins.Add(new Skin("Black", champions[5]));
			skins.Add(new Skin("Golden", champions[5]));
			skins.Add(new Skin("Matador", champions[5]));
		}

		public class SkinsManager : ISkinsManager
        {
            private readonly StubData parent;

            public SkinsManager(StubData parent)
                => this.parent = parent;

            public Task<Skin?> AddItem(Skin? item)
                => parent.skins.AddItem(item);

            public Task<bool> DeleteItem(Skin? item)
                => parent.skins.DeleteItem(item);

            public Task<IEnumerable<Skin?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.skins.GetItemsWithFilterAndOrdering(
                    skin => true,
                    index, count, orderingPropertyName, descending);

            private static Func<Skin, Champion?, bool> filterByChampion = (skin, champion) => champion != null && skin.Champion.Equals(champion!);

            private static Func<Skin, string, bool> filterByName = (skin, substring) => skin.Name.Contains(substring, StringComparison.InvariantCultureIgnoreCase);

            public Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.skins.GetItemsWithFilterAndOrdering(
                    skin => filterByChampion(skin, champion),
                    index, count, orderingPropertyName, descending);

            public Task<IEnumerable<Skin?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.skins.GetItemsWithFilterAndOrdering(
                    skin => filterByName(skin, substring),
                    index, count, orderingPropertyName, descending);

            public Task<int> GetNbItems()
                => parent.skins.GetNbItemsWithFilter(
                    c => true);

            public Task<int> GetNbItemsByChampion(Champion? champion)
                => parent.skins.GetNbItemsWithFilter(
                    skin => filterByChampion(skin, champion));

            public Task<int> GetNbItemsByName(string substring)
                => parent.skins.GetNbItemsWithFilter(
                    skin => filterByName(skin, substring));

            public Task<Skin?> UpdateItem(Skin? oldItem, Skin? newItem)
                => parent.skins.UpdateItem(oldItem, newItem);

            public async Task<Skin?> GetItemByName(string name)
            {
                return parent.skins.FirstOrDefault(skin => skin.Name == name);
            }
        }
	}
}

