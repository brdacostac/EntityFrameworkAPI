using DTOLol;
using System.Net.Http.Json;
using Shared;
using Model;
using System.Net;
using DTOLol.Factory;
using MapperApi.Mapper;

namespace ClientApi
{
    public class ChampionHttpManager: HttpGeneralManager, IChampionsManager 
    {
        private const string UrlApiChampions = "/api/Champions";

        public ChampionHttpManager(HttpClient client) : base(client){ }


        public async Task<Champion?> AddItem(Champion item)
        {
            var champion = await _client.PostAsJsonAsync($"{UrlApiChampions}", item.ToDto());
            return champion.StatusCode == HttpStatusCode.OK ? item : null;
        }

        public async Task<Champion?> UpdateItem(Champion oldItem, Champion newItem)
        {
            var champion = await _client.PutAsJsonAsync($"{UrlApiChampions}/{oldItem.Name}", newItem.ToDto());
            return champion.StatusCode == HttpStatusCode.OK ? newItem : null;
        }

        public async Task<bool> DeleteItem(Champion item)
        {
            var champions = await _client.DeleteAsync($"{UrlApiChampions}/{item.Name}"); 
            return champions.StatusCode == HttpStatusCode.OK;
        }

        public async Task<Champion?> GetItemByName(string name)
        {
            var response = await _client.GetAsync($"{UrlApiChampions}/{name}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var champions = await response.Content.ReadFromJsonAsync<DTOMessage<DTOChampion>>();
                return champions.Data.ToChampion();
            }
            return null;
        }

        public async Task<IEnumerable<Champion>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?startIndex={index}&count={count}&descending={descending}");
            var items = dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion?>> GetItemsByCharacteristic(string charName, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?characteristic={charName}&index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion?>> GetItemsByClass(ChampionClass championClass, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?championClass={championClass}&index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?name={substring}&index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion?>> GetItemsByRunePage(RunePage? runePage, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
            //var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?Name={runePage}&Index={index}&Count={count}&Descending={descending}");
            //return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion?>> GetItemsBySkill(string skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?skillName={skill}&index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<IEnumerable<Champion?>> GetItemsBySkill(Skill? skill, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOChampion>>>($"{UrlApiChampions}?skill={skill.Name}&index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToChampion()).ToList();
        }

        public async Task<int> GetNbItems()
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiChampions}/count");
        }

        public async Task<int> GetNbItemsByCharacteristic(string charName)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiChampions}/count?charName={charName}");
        }

        public async Task<int> GetNbItemsByClass(ChampionClass championClass)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiChampions}/count?class={championClass}");
        }

        public async Task<int> GetNbItemsByName(string substring)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiChampions}/count?name={substring}");
        }

        public async Task<int> GetNbItemsBySkill(string skill)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiChampions}/count?skillName={skill}");
        }

        public Task<int> GetNbItemsBySkill(Skill? skill)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByRunePage(RunePage? runePage)
        {
            throw new NotImplementedException();
        }
    }
}
