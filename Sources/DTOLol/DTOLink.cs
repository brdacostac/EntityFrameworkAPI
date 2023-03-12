using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol
{
    public class DTOLink
    {
        private string v;

        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public DTOLink(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public DTOLink(string v)
        {
            this.v = v;
        }
    }
}
