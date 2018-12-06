using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Library.Parser.Interfaces;

namespace Library.Parser.Services
{
    public class EksmoParser : IEksmoParser
    {
        private readonly IParser _parser;

        public EksmoParser(IParser parser)
        {
            _parser = parser;
        }

        public List<ParserBookModel> GetEksmo()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<HtmlNode> roots = new List<HtmlNode>();
            for (int i = 1; i < 6; i++)
            {
                var root = _parser.GetRootNode(
                    $"https://eksmo.ru/professionalnaia-literatura/kompyuternaya-literatura/page{i}/");
                roots.Add(root);
            }

            var bookNodes = GetBooksFromMainPage(roots);
            var bookLinks = GetBookLinks(bookNodes);

            var books = new List<ParserBookModel>();
            foreach (var link in bookLinks)
            {
                var book = GetEksmoBook(link);
                books.Add(book);
            }

            return books;
        }

        public ParserBookModel GetEksmoBook(string uri)
        {
            try
            {
                ParserBookModel result = new ParserBookModel();
                var root = _parser.GetRootNode(uri);

                var imageUri = root
                    .Descendants().First(a => a.GetAttributeValue("class", "").Contains("cont-book-pic"))
                    .Descendants("a").First().GetAttributeValue("href", "");

                var annotation = _parser.GetNormalizedString(
                    root.Descendants()
                        .First(a => a.GetAttributeValue("class", "").Contains("spoiler__text t annotation")).InnerHtml);

                var cost = root.Descendants()
                               .First(a => a.GetAttributeValue("class", "").Contains("price__number")).InnerHtml +
                           root.Descendants()
                               .First(a => a.GetAttributeValue("class", "").Contains("price__currency")).InnerHtml;

                var title = root.Descendants()
                    .First(a => a.GetAttributeValue("class", "").Contains("h h_2 h_no-offset book-detail-h1"))
                    .InnerHtml;

                var authorName = root.Descendants()
                    .First(a => a.GetAttributeValue("class", "").Contains("author"))
                    .Descendants("a").First().InnerText;

                var authorLink = root.Descendants()
                    .First(a => a.GetAttributeValue("class", "").Contains("author"))
                    .Descendants("a").First().Attributes["href"].Value;

                var technology = root.Descendants()
                    .First(a => a.GetAttributeValue("class", "").Contains("a a_orange series_link")).InnerText;

                string yearInfo = "";

                try
                {
                    yearInfo = root.Descendants()
                        .First(a => a.GetAttributeValue("class", "").Contains("content-text-title"))
                        .ParentNode.Descendants().Last().InnerText;

                    if (yearInfo.ToUpper() == "ПРЕДЗАКАЗ")
                    {
                        yearInfo = "2019";
                    }
                    else
                    {
                        DateTime year = Convert.ToDateTime(yearInfo);
                        yearInfo = year.Year.ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    yearInfo = "2019";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return null;
        }

        private List<HtmlNode> GetBooksFromMainPage(List<HtmlNode> roots)
        {
            List<HtmlNode> result = new List<HtmlNode>();

            foreach (var root in roots)
            {
                var nodes = root.Descendants("a")
                    .Where(a => a.GetAttributeValue("class", "").Contains("book__link")).ToList();
                result.AddRange(nodes);
            }


            return result;
        }

        public List<string> GetBookLinks(List<HtmlNode> nodes)
        {
            List<string> links = new List<string>();
            foreach (var node in nodes)
            {
                var link = node.Attributes["href"].Value;
                links.Add("https://eksmo.ru" + link);
            }

            return links;
        }
    }
}