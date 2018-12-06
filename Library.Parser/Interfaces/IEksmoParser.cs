using System.Collections.Generic;

namespace Library.Parser.Interfaces
{
    public interface IEksmoParser
    {
        List<ParserBookModel> GetEksmo();

        ParserBookModel GetEksmoBook(string uri);
    }
}