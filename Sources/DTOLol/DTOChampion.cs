using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol
{
    public class DTOChampion
    {

        public String Name
        {
            get; set;
        }

        public String Bio
        {
            get;set;
        }

        public String Class
        {
            get; set;
        }

        public String Icon
        {
            get; set;
        }

        public String Image
        {
            get; set;
        }

        public IEnumerable<DTOSkin> skins { get; set; }

    }
}
