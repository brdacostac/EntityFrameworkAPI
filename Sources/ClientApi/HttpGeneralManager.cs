﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class HttpGeneralManager
    {
        protected readonly HttpClient _client;

        public HttpGeneralManager(HttpClient client)
        {
            _client = client;
            client.BaseAddress = new Uri("https://localhost:7091");
        }
    }
}
