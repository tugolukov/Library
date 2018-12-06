using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Parser.Interfaces
{
    public interface IPiterParser
    {
        List<ParserBookModel> GetPiter();

        ParserBookModel GetPiterBook(string uri);
    }
} 