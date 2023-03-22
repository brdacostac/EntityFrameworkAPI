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
            var skins = await _client.DeleteAsync($"{UrlApiSkins}/{item.Name}");
            return skins.StatusCode == HttpStatusCode.OK;
        }

        public async Task<Skin?> GetItemByName(string name)
        {
            var skins = await _client.GetFromJsonAsync<DTOMessage<DTOSkin>>($"{UrlApiSkins}/{name}");
            return skins.Data.ToSkin();
        }

        public async Task<IEnumerable<Skin?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoSkins = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}");
            return dtoSkins.Data.Select(skin => skin.ToSkin()).ToList();
        }

        public async Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoChampions = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}&champion={champion.Name}");
            return dtoChampions.Data.Select(champion => champion.ToSkin()).ToList();
        }

        public async Task<IEnumerable<Skin?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoSkins = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?name={substring}&index={index}&count={count}&descending={descending}");
            return dtoSkins.Data.Select(skin => skin.ToSkin()).ToList();
        }

        public async Task<Skin?> UpdateItem(Skin? oldItem, Skin? newItem)
        {
            await _client.PutAsJsonAsync($"{UrlApiSkins}/{oldItem.Name}", newItem.ToDto());
            return newItem;
        }
        public async Task<int> GetNbItems()
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiSkins}/count");
        }

        public async Task<int> GetNbItemsByChampion(Champion? champion)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiSkins}/count?champion={champion}");
        }

        public async Task<int> GetNbItemsByName(string substring)
        {
            return await _client.GetFromJsonAsync<int>($"{UrlApiSkins}/count?name={substring}");
        }
    }
}
