using MapperApi.Mapper;
using DTOLol;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClientApi
{
    public class SkinHttpManager : HttpGeneralManager, ISkinsManager
    {

        private const string UrlApiSkins = "/api/Skins";
        private const string UrlApiChampions = "/api/Champions";
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
            var skin = await _client.GetFromJsonAsync<DTOMessage<DTOSkin>>($"{UrlApiSkins}/{name}");
            var champion = await _client.GetFromJsonAsync<DTOMessage<DTOChampion>>($"{UrlApiChampions}/{skin.Data.ChampionName}");
            return skin.Data.ToSkin(champion.Data.ToChampion());
        }

        public async Task<IEnumerable<Skin>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoSkins = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}");
            var skins = new List<Skin>();
            foreach (var skin in dtoSkins.Data)
            {
                var champion = await _client.GetFromJsonAsync<DTOMessage<DTOChampion>>($"{UrlApiChampions}/{skin.ChampionName}"); 
                skins.Add(skin.ToSkin(champion.Data.ToChampion()));
            }
            return skins;
        }


        public async Task<IEnumerable<Skin?>> GetItemsByChampion(Champion? champion, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoSkins = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?index={index}&count={count}&descending={descending}&champion={champion.Name}");
            var skins = new List<Skin>();
            foreach (var skin in dtoSkins.Data)
            {
                var champ = await _client.GetFromJsonAsync<DTOMessage<DTOChampion>>($"{UrlApiChampions}/{skin.ChampionName}");
                skins.Add(skin.ToSkin(champ.Data.ToChampion()));
            }
            return skins;
        }

        public async Task<IEnumerable<Skin?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            var dtoSkins = await _client.GetFromJsonAsync<DTOMessage<IEnumerable<DTOSkin>>>($"{UrlApiSkins}?name={substring}&index={index}&count={count}&descending={descending}");
            var skins = new List<Skin>();
            var championHttpManager = new ChampionHttpManager(_client);
            foreach (var skin in dtoSkins.Data)
            {
                var champ = await championHttpManager.GetItemByName(skin.ChampionName);
                skins.Add(skin.ToSkin(champ));
            }
            return skins;
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
