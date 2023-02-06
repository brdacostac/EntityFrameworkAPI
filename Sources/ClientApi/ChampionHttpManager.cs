using DTOLol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using Shared;

using System.Threading.Tasks;

namespace ClientApi
{
    public class ChampionHttpManager: IGenericDataManager<DTOChampion>
    {
        private const string UrlApiChampions = "/api/Champion";
        private readonly HttpClient _client;

        public ChampionHttpManager(HttpClient client) {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7091");
        }

        public Task<DTOChampion> AddItem(DTOChampion item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(DTOChampion item)
        {
            throw new NotImplementedException();
        }

      
        public async Task<IEnumerable<DTOChampion>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var champions = await _client.GetFromJsonAsync<IEnumerable<DTOChampion>>(UrlApiChampions);
            return champions;
        }

        public Task<IEnumerable<DTOChampion>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItems()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByName(string substring)
        {
            throw new NotImplementedException();
        }

        public Task<DTOChampion> UpdateItem(DTOChampion oldItem, DTOChampion newItem)
        {
            throw new NotImplementedException();
        }
    }
}
