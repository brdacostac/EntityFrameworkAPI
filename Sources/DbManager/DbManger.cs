using Entity_framework;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class DbManger : IDataManager
    {
        protected EntityDbContexte DbContext;
        public DbManger(EntityDbContexte dbContext)
        {
            DbContext = dbContext;
            ChampionsMgr = new DbChampionManger(this);
            SkinsMgr = new DbSkinManger(this);
            RunesMgr = new DbRunesManger(this);
            RunePagesMgr = new DbRunePagesManger(this);
        }
        public IChampionsManager ChampionsMgr { get; set; }

        public ISkinsManager SkinsMgr { get; }

        public IRunesManager RunesMgr { get; }

        public IRunePagesManager RunePagesMgr { get; }
    }
}
