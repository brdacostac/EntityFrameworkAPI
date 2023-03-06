using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol.Factory
{
    public static class FactoryMessage
    {
        public static DTOMessage<T> MessageCreate<T>(string message, T data) => new DTOMessage<T>(message,data);
        public static DTOMessage MessageCreate(string message) => new DTOMessage(message);
    }
}
