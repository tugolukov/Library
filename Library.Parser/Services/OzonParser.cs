using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Library.Parser.Interfaces;

namespace Library.Parser.Services
{
    public class OzonParser : IOzonParser
    {
        
        public Task GetOzon()
        {
            // Включаем кодировки
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Берем html из файла в корне
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(GetHtmlFromFile());
            var root = htmlDocument.DocumentNode;
            
            // Парсим каталог
            ParseCatalog(root);
            
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Разбор каталога
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        private Task ParseCatalog(HtmlNode rootNode)
        {
            // Собираем все обложки
            var nodes = rootNode.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("full-cover-link")).ToList();

            foreach (var node in nodes)
            {
                // Собираем ссылки на книги и начинаем разбирать по очереди
                var innerLink = node.Attributes["href"].Value;
                ParseBook(innerLink);
            }
            return null;
        }

        /// <summary>
        /// Разбор книги
        /// </summary>
        /// <param name="uri">Ссылка на книгу</param>
        /// <returns></returns>
        private Task ParseBook(string uri)
        {
            var root = GetRootNode(uri);
            //CreateBookModel createBookModel = new CreateBookModel();
            
            // Разбираем раздел характеристик
            var nodes = root.Descendants()
                .Where(n => n.GetAttributeValue("class", "").Equals("eItemProperties_line")).ToList();

            foreach (var node in nodes)
            {
                if (IsAuthorNode(node))
                {
//                     createBookModel.AuthorGuid = await SetAuthor(node);
                }
                else if (IsFormat(node))
                {
//                    var format = await SetFormat(node);
//                    createBookModel.CoverType = format.CoverType;
//                    createBookModel.Format = format.Format;
                }
            }
            return null;
        }

        /// <summary>
        /// Парсинг данных автора
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private object GetPersonInfo(string uri)
        {           
            var htmlDocument = new HtmlDocument();
            var htmlString = GetStringFromUri(uri);

            string name = "";
            string description = "";
            
            htmlDocument.LoadHtml(htmlString);

            var root = htmlDocument.DocumentNode;
            var nodes = root.Descendants()
                .Where(n => n.GetAttributeValue("class", "").Equals("eNonGoodPopup_ContentBlock ")).ToList();

            foreach (var node in nodes)
            {
                var innerNodes = node.Descendants()
                    .Where(n => n.GetAttributeValue("class", "").Equals("eNonGoodPopup_TopInfo")).ToList();

                foreach (var innerNode in innerNodes)
                {
                    var nameNode = innerNode.Descendants()
                        .Where(u => u.GetAttributeValue("class", "").Equals("eNonGoodPopup_Line")).ToList().First();

                    name = nameNode.InnerHtml;
                }

                try
                {
                    description = node.Descendants()
                        .Where(d => d.GetAttributeValue("itemprop", "").Equals("description")).ToList().First().InnerHtml;
                }
                catch (Exception)
                {
                    description = "Информация об авторе отсутствует.";
                }

            }

            if (String.IsNullOrEmpty(description))
            {
//                return new AuthorParse(name);
            }
//            return new AuthorParse(name, description);
            return null;
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
            
            StreamReader streamReader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(1251));
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

        /// <summary>
        /// Получение html-строки из файла(хардкод)
        /// </summary>
        /// <returns></returns>
        private string GetHtmlFromFile()
        {
            var path = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(path, "wwwroot\\files\\parser\\ozon.html");         

            StreamReader streamReader = new StreamReader(fullPath, System.Text.Encoding.GetEncoding(1251));
            var fullhtml = streamReader.ReadToEnd();
            streamReader.Close();

            return fullhtml;
        }

        /// <summary>
        /// Проверка поля c автором
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsAuthorNode(HtmlNode node)
        {           
            return IsNode(node, "author");
        }

        /// <summary>
        /// Проверка поля с форматом книги
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsFormat(HtmlNode node)
        {
            return IsNode(node, "bookFormat");
        }
        
        /// <summary>
        /// Добавление авторов в базу данных
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
//        private async Task<Guid> SetAuthor(HtmlNode node)
//        {
//            Guid resultGuid = Guid.Empty;
//            var data = node.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("eItemProperties_text"))
//                .ToList().First();
//            var itemprop = data.Attributes["itemprop"];
//
//            var item = itemprop.OwnerNode;
//            var hrefs = item.Descendants("a").ToList();
//            
//            // Если авторов несколько, должен добавить несколько
//            foreach (var href in hrefs)
//            {
//                var authorLink = "https://www.ozon.ru" + href.Attributes["href"].Value;
//                var authorParse = GetPersonInfo(authorLink);
//                
//                CreatePersonModel personModel = new CreatePersonModel();
//                personModel.Surname = authorParse.Surname;
//                personModel.Name = authorParse.Name;
//                personModel.Patronymic = authorParse.Patronymic;
//                personModel.Note = authorParse.Notes;
//
//                var personGuid = await _personsService.Create(personModel);
//                var person = await _personsService.Read(personGuid);
//                var author = await _rolesService.CreateAuthor(person);
//                resultGuid = author.AuthorGuid;
//            }
//            
//            return resultGuid;
//        }
//
//        private async Task<FormatParse> SetFormat(HtmlNode node)
//        {
//            FormatParse formatParse = new FormatParse();
//            
//            var data = node.Descendants().Where(n => n.GetAttributeValue("class", "").Equals("eItemProperties_text"))
//                .ToList();
//            foreach (var item in data)
//            {
//                var itemprop = item.Attributes["itemprop"];
//                
//            }
//
//
//            return formatParse;
//        }
//        
        /// <summary>
        /// Проверка поля с itemprop
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool IsNode(HtmlNode node, string parameter)
        {
            if (node.Descendants().Where(a => a.GetAttributeValue("itemprop", "").Equals(parameter)).ToList().Count != 0)
            {
                return true;
            }

            return false;
        }
    }
}