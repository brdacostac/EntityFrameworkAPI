using MapperApi.Mapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class RunePagesHttpManager : HttpGeneralManager, IRunePagesManager
    {
        private const string UrlApiRunePages = "/api/v1/RunePages";

        public RunePagesHttpManager(HttpClient client) : base(client) { }
        public async Task<RunePage?> AddItem(RunePage? item)
        {
            await _client.PostAsJsonAsync($"{UrlApiRunePages}", item.ToDto());
            return item;
        }

        public async Task<bool> DeleteItem(RunePage? item)
        {
            var champions = await _client.DeleteAsync($"{UrlApiRunePages}/{item.Name}");
            return champions.StatusCode == HttpStatusCode.OK;
        }

        public Task<RunePage?> GetItemByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RunePage?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RunePage?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RunePage?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RunePage?>> GetItemsByRune(Model.Rune? rune, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        public async Task<RunePage?> UpdateItem(RunePage? oldItem, RunePage? newItem)
        {
            await _client.PutAsJsonAsync($"{UrlApiRunePages}/{oldItem.Name}", newItem.ToDto());
            return newItem;
        }

        //Nb
        public Task<int> GetNbItems()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByChampion(Champion? champion)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByName(string substring)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByRune(Model.Rune? rune)
        {
            throw new NotImplementedException();
        }
    }
}
