using DTOLol;
using System.Net.Http.Json;
using Shared;
using Model;
using Api.Mapper;

namespace ClientApi
{
    public class ChampionHttpManager: IChampionsManager
    {
        private const string UrlApiChampions = "/api/Champions";
        private readonly HttpClient _client;

        public ChampionHttpManager(HttpClient client) {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7091");
        }

        public async Task<Champion> AddItem(Champion item)
        {
            var resp=await _client.PostAsJsonAsync($"{UrlApiChampions}", item.ToDto());

            return item;
        }

        public async Task<bool> DeleteItem(Champion item)
        {

            var champions = await _client.DeleteAsync($"{UrlApiChampions}/{item.Name}"); //a finire
            return true;
        }

        public async Task<Champion> GetItemByName(string name)
        {
            var champions = await _client.GetFromJsonAsync<DTOChampion>($"{UrlApiChampions}/{name}");
            return champions.ToChampion();
        }

        public async Task<IEnumerable<Champion>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<IEnumerable<DTOChampion>>(UrlApiChampions);
            List<Champion> champions = (List<Champion>)dtoChampions.Select(champion => champion.ToChampion());
            return champions;
        }

        public Task<IEnumerable<Champion?>> GetItemsByCharacteristic(string charName, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Champion?>> GetItemsByClass(ChampionClass championClass, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Champion>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Champion?>> GetItemsByRunePage(RunePage? runePage, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Champion?>> GetItemsBySkill(string skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Champion?>> GetItemsBySkill(Skill? skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItems()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByCharacteristic(string charName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByClass(ChampionClass championClass)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByName(string substring)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByRunePage(RunePage? runePage)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsBySkill(Skill? skill)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsBySkill(string skill)
        {
            throw new NotImplementedException();
        }

        public Task<Champion> UpdateItem(Champion oldItem, Champion newItem)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Champion?>> IGenericDataManager<Champion?>.GetItems(int index, int count, string? orderingPropertyName, bool descending)
        {
            throw new NotImplementedException();
        }
    }
}
