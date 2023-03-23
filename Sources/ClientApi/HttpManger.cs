using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class HttpManger : IDataManager
    {
        protected HttpClient client;
        public HttpManger()
        {
            client= new HttpClient();
            ChampionsMgr = new ChampionHttpManager(client);
            SkinsMgr = new SkinHttpManager(client);
            RunesMgr = new RuneHttpManager(client);
            RunePagesMgr = new RunePagesHttpManager(client);
        }
        public IChampionsManager ChampionsMgr { get; set; }

        public ISkinsManager SkinsMgr { get; }

        public IRunesManager RunesMgr { get; }

        public IRunePagesManager RunePagesMgr { get; }
    }
}
