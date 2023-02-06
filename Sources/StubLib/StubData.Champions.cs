using System;
using Model;

namespace StubLib
{
	public partial class StubData
	{
		private List<Champion> champions = new()
		{
			new Champion("Akali", ChampionClass.Assassin),
			new Champion("Aatrox", ChampionClass.Fighter),
			new Champion("Ahri", ChampionClass.Mage),
			new Champion("Akshan", ChampionClass.Marksman),
			new Champion("Bard", ChampionClass.Support),
			new Champion("Alistar", ChampionClass.Tank),
		};

		public class ChampionsManager : IChampionsManager
        {
            private readonly StubData parent;

            public ChampionsManager(StubData parent)
                => this.parent = parent;

            public Task<Champion?> AddItem(Champion? item)
                => parent.champions.AddItem(item);

            public Task<bool> DeleteItem(Champion? item)
                => parent.champions.DeleteItem(item);

            public Task<int> GetNbItems()
                => Task.FromResult(parent.champions.Count);

            public Task<IEnumerable<Champion?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(
                        c => true,
                        index, count,
                        orderingPropertyName, descending);

            private Func<Champion, string, bool> filterByCharacteristic = (champ, charName) => champ.Characteristics.Keys.Any(k => k.Contains(charName, StringComparison.InvariantCultureIgnoreCase));

            public Task<int> GetNbItemsByCharacteristic(string charName)
                => parent.champions.GetNbItemsWithFilter(champ => filterByCharacteristic(champ, charName));

            public Task<IEnumerable<Champion?>> GetItemsByCharacteristic(string charName, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(
                        champ => filterByCharacteristic(champ, charName),
                        index, count, orderingPropertyName, descending);

            private Func<Champion, ChampionClass, bool> filterByClass = (champ, championClass) => champ.Class == championClass;

            public Task<int> GetNbItemsByClass(ChampionClass championClass)
                => parent.champions.GetNbItemsWithFilter(
                    champ => filterByClass(champ, championClass));

            public Task<IEnumerable<Champion?>> GetItemsByClass(ChampionClass championClass, int index, int count, string? orderingPropertyName, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(
                    champ => filterByClass(champ, championClass),
                    index, count, orderingPropertyName, descending);

            private Func<Champion, Skill?, bool> filterBySkill = (champ, skill) => skill != null && champ.Skills.Contains(skill!);

            public Task<int> GetNbItemsBySkill(Skill? skill)
                => parent.champions.GetNbItemsWithFilter(champ => filterBySkill(champ, skill));

            public Task<IEnumerable<Champion?>> GetItemsBySkill(Skill? skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(champ => filterBySkill(champ, skill), index, count, orderingPropertyName, descending);

            private static Func<Champion, string, bool> filterBySkillSubstring = (champ, skill) => champ.Skills.Any(s => s.Name.Contains(skill, StringComparison.InvariantCultureIgnoreCase));

            public Task<int> GetNbItemsBySkill(string skillSubstring)
                => parent.champions.GetNbItemsWithFilter(champ => filterBySkillSubstring(champ, skillSubstring));

            public Task<IEnumerable<Champion?>> GetItemsBySkill(string skillSubstring, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(champ => filterBySkillSubstring(champ, skillSubstring), index, count, orderingPropertyName, descending);

            public Task<int> GetNbItemsByRunePage(RunePage? runePage)
                => Task.FromResult(parent.championsAndRunePages.Count(tuple => tuple.Item2.Equals(runePage)));

            public Task<IEnumerable<Champion?>> GetItemsByRunePage(RunePage? runePage, int index, int count, string? orderingPropertyName = null, bool descending = false)
                => Task.FromResult<IEnumerable<Champion?>>
                            (parent.championsAndRunePages
                                    .Where(tuple => tuple.Item2.Equals(runePage))
                                    .Select(tuple => tuple.Item1)
                                    .Skip(index*count).Take(count));

            private Func<Champion, string, bool> filterByName = (champ, substring) => champ.Name.Contains(substring, StringComparison.InvariantCultureIgnoreCase);

            public Task<int> GetNbItemsByName(string substring)
                => parent.champions.GetNbItemsWithFilter(champ => filterByName(champ, substring));


            public async Task<Champion?> GetItemByName(string name)
            {
                foreach(var champion in parent.champions)
                {
                    if(champion.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return champion;
                    }
                }
                return null;
            }

            public Task<IEnumerable<Champion?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName, bool descending = false)
                => parent.champions.GetItemsWithFilterAndOrdering(champ => filterByName(champ, substring), index, count, orderingPropertyName, descending);

            public Task<Champion?> UpdateItem(Champion? oldItem, Champion? newItem)
                => parent.champions.UpdateItem(oldItem, newItem);

           
        }
	}
}

