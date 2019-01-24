using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Library.Domain.Interfaces;
using Library.Domain.Models.Search;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Domain.Services
{
    public class GlobalSearchService : IGlobalSearchService
    {
        private readonly IBooksService _booksService;

        public GlobalSearchService(IBooksService booksService)
        {
            _booksService = booksService;
        }

        public async Task<List<SearchResult>> Search(string param)
        {         
            List<string> allParameters = param.Split(new [] {' ', ';', ','}).ToList();
            List<string> parameters = new List<string>();
            List<SearchResult> urls = new List<SearchResult>();
            string baseUrl = "http://itlibrary.site/books/";
            double increment = 0.05;

            foreach (var item in allParameters)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    parameters.Add(item);
                }
            }

            if (parameters.Count == 0)
            {
                return urls;
            }
            
            if (param.ToUpper().Contains("СЕРВИС")
                ||param.ToUpper().Contains("ВИДЖЕТ")
                ||param.ToUpper().Contains("ПОЛЕЗНОЕ"))
            {
                var services = new SearchResult()
                {
                    Title = "Сервисы",
                    Description = "На странице представлены виджеты для удобного представления погоды, времени и прочих данных. Перейдите по ссылке, чтобы воспользоваться",
                    Probability = 10.0,
                    Uri = "http://itlibrary.site/services/"
                };
                urls.Add(services);
            }

            if (param.ToUpper().Contains("XML"))
            {
                var services = new SearchResult()
                {
                    Title = "XML",
                    Description = "XML-представление содержимого базы данных. Перейдите для просмотра.",
                    Probability = 10.0,
                    Uri = "http://itlibrary.site/xml/"
                };
                urls.Add(services);
            }

            if (param.ToUpper().Contains("НОВОСТИ") ||
                param.ToUpper().Contains("RSS"))
            {
                var services = new SearchResult()
                {
                    Title = "RSS-новости",
                    Description = "Новостные каналы. Добавляй существующие, создавай свои и просматривай в удобном виде.",
                    Probability = 10.0,
                    Uri = "http://itlibrary.site/rss/"
                };
                urls.Add(services);
            }
            
            var books = await _booksService.ReadAll();

            foreach (var book in books)
            {
                // Проверяем книгу на вхождение параметров
                SearchResult findBook = new SearchResult();
                findBook.Probability = 0.0;
                var bookUri = baseUrl + book.BookGuid.ToString();

                // Тут можно выгрузить описание со страницы
                var root = GetRootNode(bookUri);
                var node = root.Descendants("meta").ToList();
                var description = node[3].Attributes[1].DeEntitizeValue.ToUpper(); // костыль лютый, но шо поделать
                
                foreach (var parameter in parameters)
                {
                    if (description.Contains(parameter.ToUpper()))
                    {
                        int count = 1;
                        count = count + description.Split(new string[] {parameter.ToUpper()}, StringSplitOptions.None).Count() - 1;
                        
                        findBook.Probability = (findBook.Probability + increment) * count;
                        findBook.Uri = bookUri;
                        findBook.Title = book.Title;
                        findBook.Description = book.Annotation;
                    }
                }

                if (findBook.Probability > 0.0)
                {
                    urls.Add(findBook);
                }

                findBook = null;
            }

            urls = urls.OrderByDescending(a => a.Probability).ToList();
            
            return urls;
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
        
    }
}