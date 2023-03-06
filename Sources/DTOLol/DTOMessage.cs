using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol
{
    public class DTOMessage<T>
    {
        public string Message { get; set; }

        public T Data { get; set; }

        public DTOMessage(string message, T data = default)
        {
            Message = message;
            Data = data;
        }

    }
}
