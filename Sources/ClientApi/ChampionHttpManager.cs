using DTOLol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class ChampionHttpManager 
    {
        private const string UrlApiChampions = "/api/Champion";
        private readonly HttpClient _client;

        public ChampionHttpManager(HttpClient client) {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7091");
        }

        public async Task<IEnumerable<DTOChampion>> GetChampions(){
            var champions = await _client.GetFromJsonAsync<IEnumerable<DTOChampion>>(UrlApiChampions);
            return champions;
        }

    }
}
