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
        public int? CurrentPage { get; set; }
        public int? NextPage { get; set; }
        public int? TotalPages { get; set; }
        public int? TotalCount { get; set; }

        public DTOMessage(string message, T data = default)
        {
            Message = message;
            Data = data;
        }


        public DTOMessage(string message, int currentPage, int nextPage, int totalPages, int totalItemCount, T list = default)
        {
            Message = message;
            CurrentPage = currentPage;
            NextPage = nextPage;
            TotalPages = totalPages;
            TotalCount = totalItemCount;
            Data = list;
        }

    }

    public class DTOMessage
    {
        public string Message { get; set; }
        public List<DTOLink> Links { get; set; }

        public DTOMessage(string message)
        {
            Message = message;
        }

        

        public DTOMessage(string message, List<DTOLink> links)
        {
            Message = message;
            Links = links;
        }

    }
}
