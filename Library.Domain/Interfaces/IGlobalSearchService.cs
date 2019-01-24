using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Search;

namespace Library.Domain.Interfaces
{
    public interface IGlobalSearchService
    {
        Task<List<SearchResult>> Search(string param);
    }
}