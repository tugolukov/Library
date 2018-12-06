using HtmlAgilityPack;

namespace Library.Parser.Interfaces
{
    public interface IParser
    {
        HtmlNode GetRootNode(string uri);

        string GetStringFromUri(string uri);

        string GetNormalizedString(string str);
    }
}