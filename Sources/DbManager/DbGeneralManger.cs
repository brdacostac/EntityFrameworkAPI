using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class DbGeneralManger
    {
        protected readonly DbManger parent;

        public DbGeneralManger(DbManger parent) => this.parent = parent;
    }
}
