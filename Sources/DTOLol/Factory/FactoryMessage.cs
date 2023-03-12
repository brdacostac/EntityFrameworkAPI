using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLol.Factory
{
    public static class FactoryMessage
    {
        public static string MessageCreateReponsive<T>(string message, T data)
        {
            var jsonSettings = new JsonSerializerSettings { Converters = new[] { new JsonConverterMessage() } };

            var jsonData = JsonConvert.SerializeObject(MessageCreate<T>(message,data), jsonSettings);
            return jsonData;
        }
        public static DTOMessage<T> MessageCreate<T>(string message, T data) => new DTOMessage<T>(message, data);
        public static DTOMessage MessageCreate(string message) => new DTOMessage(message);

        public static DTOMessage MessageCreate(string message,List<DTOLink> list) => new DTOMessage(message, list);

        public static DTOMessage<T> MessageCreate<T>(string message, int currentPage, int nextPage, int totalPages, int totalItemCount, T list) => new DTOMessage<T>(message, currentPage, nextPage, totalPages, totalItemCount, list);
    }
}
