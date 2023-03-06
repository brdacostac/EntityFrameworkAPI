using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class HttpManager
    {
        protected readonly HttpClient _client;

        public HttpManager(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7091");
        }
    }
}
