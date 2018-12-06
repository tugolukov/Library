using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Library.Parser.Interfaces;

namespace Library.Parser.Services
{
    public class PiterParser : IPiterParser
    {
        public List<ParserBookModel> GetPiter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var root = GetRootNode(
                "https://www.piter.com/collection/kompyutery-i-internet?page_size=100&order=&q=&only_available=true");
            
            var nodes = root.Descendants("a")
                .Where(a => a.GetAttributeValue("title", "").Contains(""))
                .Where(b => b.GetAttributeValue("href", "").Contains("/collection/kompyutery-i-internet/product"))
                .ToList();

            var links = GetBookLinks(nodes);
            var books = new List<ParserBookModel>();
            foreach (var link in links)
            {
                var book = GetPiterBook(link);
                books.Add(book);
            }
            
            return books;

        }

        public ParserBookModel GetPiterBook(string uri)
        {         
            ParserBookModel result = new ParserBookModel();
            
            var root = GetRootNode(uri);

            var imageUri = root.Descendants()
                .Where(b => b.GetAttributeValue("class", "").Contains("coverProduct")).ToList().First()
                .ParentNode.ChildNodes
                .Where(n => n.Name.Contains("img")).ToList().First()
                .GetAttributeValue("src","");
            
            var annotation = root.Descendants()
                .Where(a => a.GetAttributeValue("class", "").Contains("tabs")).ToList().First().Descendants()
                .Where(b => b.GetAttributeValue("id", "").Contains("tab-1")).ToList().First().InnerText;

            var costs = root.Descendants()
                .Where(a => a.GetAttributeValue("class", "").Contains("price"))
                .Where(b => b.Name.Contains("div"))
                .Where(c => c.InnerText.StartsWith("1") || 
                            c.InnerText.StartsWith("2") ||
                            c.InnerText.StartsWith("3") ||
                            c.InnerText.StartsWith("4") ||
                            c.InnerText.StartsWith("5") ||
                            c.InnerText.StartsWith("6") ||
                            c.InnerText.StartsWith("7") ||
                            c.InnerText.StartsWith("8") ||
                            c.InnerText.StartsWith("9")).ToList();

            var costPaper = costs.First().InnerText;
            var costDigital = costs.Last().InnerText;
                
            var bookInfoNode = GetBookInfoNode(root);
            
            var title = GetTitleBook(bookInfoNode);

            var authors = GetAuthors(bookInfoNode);
            
            var description = bookInfoNode.Descendants()
                .Where(a => a.GetAttributeValue("class", "").Contains("clearfix")).ToList();

            string technology = "";
            string year = "";
            string numberOfPages = "";
            string format = "";
            
            foreach (var item in description)
            {
                if (item.OuterHtml.ToLower().Contains("тема"))
                {
                    technology = GetNormalizedString(item.ChildNodes[3].InnerText); 
                }
                else if (item.OuterHtml.ToLower().Contains("год"))
                {
                    year = GetNormalizedString(item.ChildNodes[3].InnerText);
                }
                else if (item.OuterHtml.ToLower().Contains("страниц"))
                {
                    numberOfPages = GetNormalizedString(item.ChildNodes[3].InnerText);
                }
                else if (item.OuterHtml.ToLower().Contains("формат"))
                {
                    format = GetNormalizedString(item.ChildNodes[3].InnerText);
                }
            }

            result.Annotation = annotation;
            result.Cover = "Не указано";
            result.Format = format;
            result.Title = title;
            result.Year = year;
            result.AuthorsNames = authors;
            result.NumberOfPages = numberOfPages;
            result.CostDigital = costDigital;
            result.CostPaper = costPaper;
            result.BuyUri = uri;
            result.ImageUri = imageUri;
            result.TechnologiesName = technology;
            result.PublishingName = "ПИТЕР";
            
            
            
            return result;
        }

        /// <summary>
        /// Получение строки с удаленными пробелами и переносами строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetNormalizedString(string str)
        {
            var result = Regex.Replace(str, @"\n", string.Empty).Trim(new char[] {' '});
            return Regex.Replace(result, @"\t", string.Empty).Trim(new char[]{' '});
        }
        
        /// <summary>
        /// Получить элемент страницы с инфой о книге
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private HtmlNode GetBookInfoNode(HtmlNode node)
        {
            return node.Descendants()
                .Where(a => a.GetAttributeValue("class", "").Contains("product-info")).ToList().First();
        }
        
        /// <summary>
        /// Получение названия книги
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetTitleBook(HtmlNode node)
        {
            return node.Descendants("h1").ToList().First().InnerHtml;
        }
        
        /// <summary>
        /// Получение авторов из элемента
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<string> GetAuthors(HtmlNode node)
        {
            var authorNodes = node
                .Descendants("p")
                .First(a => a.GetAttributeValue("class", "").Contains("author")).ChildNodes
                .Where(b => b.Name == "span").ToList();
            List<string> authors = new List<string>();

            foreach (var authorNode in authorNodes)
            {
                var author = authorNode.InnerText;
                authors.Add(author);
            }

            return authors;
        }

        /// <summary>
        /// Получение списка ссылок на книги
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<string> GetBookLinks(List<HtmlNode> nodes)
        {
            List<string> links = new List<string>();
            foreach (var node in nodes)
            {
                var link = node.Attributes["href"].Value;
                links.Add("https://www.piter.com" + link);
            }
            
            return links;
        }

        /// <summary>
        /// Получение html-строки из адреса
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private string GetStringFromUri(string uri)
        {
            WebRequest request; 
            WebResponse response;
            try
            {
                request = WebRequest.Create(uri);
                request.Method = "GET";
                response = request.GetResponse();

            }
            catch (Exception)
            {
                request = WebRequest.Create(uri + "?store=1%2c0&group=div_book");
                request.Method = "GET";
                response = request.GetResponse();
            }
            
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            response.Close();
            
            return result;
        }
        
        /// <summary>
        /// Получение корневого элемента
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private HtmlNode GetRootNode(string uri)
        {
            var htmlDocument = new HtmlDocument();
            var htmlString = GetStringFromUri(uri);
            htmlDocument.LoadHtml(htmlString);

            var root = htmlDocument.DocumentNode;
            return root;
        }
    }
}