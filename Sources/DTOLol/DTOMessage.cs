using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol
{
    public class DTOMessage
    {
        public string Message { get; set; }

        public DTOMessage(string message)
        {
            Message = message;
        }

    }
}
