using Api.Mapper;
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
    public class RuneHttpManager : HttpManager, IRunesManager
    {
        private const string UrlApiRunes = "/api/Runes";

        public RuneHttpManager(HttpClient client) : base(client) { }

        //Ok
        public async Task<Model.Rune?> AddItem(Model.Rune? item)
        {
            await _client.PostAsJsonAsync($"{UrlApiRunes}", item.ToDto());
            return item;
        }

        //Ok
        public async Task<bool> DeleteItem(Model.Rune? item)
        {
            var champions = await _client.DeleteAsync($"{UrlApiRunes}/{item.Name}");
            return champions.StatusCode == HttpStatusCode.OK;
        }

        
        public Task<Model.Rune?> GetItemByName(string name)
        {
            throw new NotImplementedException();
        }

        //OK
        public Task<IEnumerable<Model.Rune?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }


        //OK
        public Task<IEnumerable<Model.Rune?>> GetItemsByFamily(RuneFamily family, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        //OK
        public Task<IEnumerable<Model.Rune?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        //OK
        public async Task<Model.Rune?> UpdateItem(Model.Rune? oldItem, Model.Rune? newItem)
        {
            await _client.PutAsJsonAsync($"{UrlApiRunes}/{oldItem.Name}", newItem.ToDto());
            return newItem;
        }


        //Nb
        public Task<int> GetNbItems()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByFamily(RuneFamily family)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByName(string substring)
        {
            throw new NotImplementedException();
        }
    }
}
