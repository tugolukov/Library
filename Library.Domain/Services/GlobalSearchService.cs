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
            List<string> parameters = param.Split(' ').ToList();
            
            string baseUrl = "http://localhost:4400/books/";

            // get a list of published articles
            var books = await _booksService.ReadAll();

            List<SearchResult> urls = new List<SearchResult>();

            foreach (var p in parameters)
            {
                string parameter = p.ToUpper();
                foreach(var book in books)
                {
                    if (book.Title.ToUpper().Contains(parameter) 
                        || book.Annotation.ToUpper().Contains(parameter) 
                        || book.AuthorModel[0].Surname.ToUpper().Contains(parameter) 
                        || book.AuthorModel[0].Name.ToUpper().Contains(parameter) 
                        || book.AuthorModel[0].Patronymic.ToUpper().Contains(parameter) 
                        || book.AuthorModel[0].Note.ToUpper().Contains(parameter) 
                        || book.PublishingModel.Name.ToUpper().Contains(parameter) 
                        || book.TechnologyModel.Name.ToUpper().Contains(parameter) 
                        || book.TechnologyModel.Description.ToUpper().Contains(parameter) 
                        || book.TechnologyModel.Language.ToUpper().Contains(parameter))
                    {
                        Console.WriteLine("plplplplpl");
                        urls.Add(
                            new SearchResult()
                            {
                                Title = book.Title,
                                Uri = baseUrl + book.BookGuid
                            });
                        Console.WriteLine("popopopo");
                    }
                }
            }
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