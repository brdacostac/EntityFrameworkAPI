using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi
{
    public class RuneHttpManager : HttpManager, IRunesManager
    {
        private const string UrlApiChampions = "/api/Runes";

        public RuneHttpManager(HttpClient client) : base(client) { }

        //Ok
        public Task<Model.Rune?> AddItem(Model.Rune? item)
        {
            throw new NotImplementedException();
        }

        //Ok
        public Task<bool> DeleteItem(Model.Rune? item)
        {
            throw new NotImplementedException();
        }

        
        public Task<Model.Rune?> GetItemByName(string name)
        {
            throw new NotImplementedException();
        }

        //OK
        public Task<IEnumerable<Model.Rune?>> GetItems(int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }


        //OK
        public Task<IEnumerable<Model.Rune?>> GetItemsByFamily(RuneFamily family, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        //OK
        public Task<IEnumerable<Model.Rune?>> GetItemsByName(string substring, int index, int count, string? orderingPropertyName = null, bool descending = false)
        {
            throw new NotImplementedException();
        }

        //OK
        public Task<Model.Rune?> UpdateItem(Model.Rune? oldItem, Model.Rune? newItem)
        {
            throw new NotImplementedException();
        }


        //Nb
        public Task<int> GetNbItems()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByFamily(RuneFamily family)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNbItemsByName(string substring)
        {
            throw new NotImplementedException();
        }
    }
}
