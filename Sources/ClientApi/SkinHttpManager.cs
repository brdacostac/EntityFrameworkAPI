using Api.Mapper;
using DTOLol;
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
    public class SkinHttpManager : HttpManager, ISkinsManager
    {



        private const string UrlApiSkins = "/api/Skins";

        public SkinHttpManager(HttpClient client) : base(client) { }
        public async Task<Skin?> AddItem(Skin? item)
        {
            await _client.PostAsJsonAsync($"{UrlApiSkins}", item.ToDto());
            return item;
        }

        public async Task<bool> DeleteItem(Skin? item)
        {
            var champions = await _client.DeleteAsync($"{UrlApiSkins}/{item.Name}");
            return champions.StatusCode == HttpStatusCode.OK;
        }

        public async Task<Skin?> GetItemByName(string name)
        {
            var champions = await _client.GetFromJsonAsync<DTOMessage<DTOSkin>>($"{UrlApiSkins}/{name}");
            return champions.Data.ToSkin();
        }

        public async Task<IEnumerable<Skin?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}");
            return dtoChampions.Data.Select(champion => champion.ToSkin()).ToList();
        }

        public async Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}&champion={champion.Name}");
            return dtoChampions.Data.Select(champion => champion.ToSkin()).ToList();
        }

        public Task<IEnumerable<Skin?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();, 
        }

        public async Task<Skin?> UpdateItem(Skin? oldItem, Skin? newItem)
        {
            await _client.PutAsJsonAsync($"{UrlApiSkins}/{oldItem.Name}", newItem.ToDto());
            return newItem;
        }
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
    }
}
