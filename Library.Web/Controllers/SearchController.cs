using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;
using Library.Web.Models.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Web.Controllers
{
    [Route("/search")]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly ITechnologiesService _technologiesService;
        private readonly IBooksService _booksService;
        private readonly IAuthorsService _authorsService;
        private readonly IGlobalSearchService _globalSearchService;

        public SearchController(
            ISearchService searchService, 
            ITechnologiesService technologiesService, 
            IBooksService booksService, 
            IAuthorsService authorsService, IGlobalSearchService globalSearchService)
        {
            _searchService = searchService;
            _technologiesService = technologiesService;
            _booksService = booksService;
            _authorsService = authorsService;
            _globalSearchService = globalSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var technologies = await _technologiesService.ReadAll();
            List<SelectListItem> technologyList = new List<SelectListItem>();
            foreach (var technology in technologies)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = technology.Name,
                    Value = technology.TechnologyGuid.ToString()
                };
                technologyList.Add(item);
            }

            ViewBag.technology = technologyList;
            
            
            List<SelectListItem> authorList = new List<SelectListItem>();
            var allBooks = await _booksService.ReadAll();
            var books = allBooks.Where(a => a.TechnologyModel.TechnologyGuid == technologies.First().TechnologyGuid).ToList();
            List<AuthorModel> authors = new List<AuthorModel>();
            foreach (var book in books)
            {
                authors.AddRange(book.AuthorModel);
            }
            
            foreach (var author in authors)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{author.Surname} {author.Name}",
                    Value = author.AuthorGuid.ToString()
                };
                authorList.Add(item);
            }

            ViewBag.author = authorList;
            
            return View();
        }

        [HttpGet("technologies")]
        public async Task<PartialViewResult> GetTechnologies(string id)
        {
            var technologies = new List<TechnologyModel>();
            if (String.IsNullOrEmpty(id))
            {
                technologies = await _technologiesService.ReadAll();
            }
            else
            {
                var allBooks = await _booksService.ReadAll();
                var books = allBooks.Where(a => a.AuthorGuid.Contains(Guid.Parse(id))).ToList();
            
                var items = new List<TechnologyModel>();
                foreach (var book in books)
                {
                    items.Add(book.TechnologyModel);
                }

                foreach (var item in items)
                {
                    if (technologies.All(a => a.TechnologyGuid != item.TechnologyGuid))
                    {
                        technologies.Add(item);
                    }
                }         
            }
            
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var technology in technologies)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = technology.Name,
                    Value = technology.TechnologyGuid.ToString()
                };
                result.Add(item);
            }

            return PartialView(result);
        }

        [HttpGet("authors")]
        public async Task<PartialViewResult> GetAuthors(string id)
        {
            var authors = new List<AuthorModel>();
            if (String.IsNullOrEmpty(id))
            {
                authors = await _authorsService.ReadAll();
            }
            else
            {
                var allBooks = await _booksService.ReadAll();
                var books = allBooks.Where(a => a.TechnologyModel.TechnologyGuid == Guid.Parse(id)).ToList();
                List<AuthorModel> items = new List<AuthorModel>();
                foreach (var book in books)
                {
                    items.AddRange(book.AuthorModel);
                }

                foreach (var item in items)
                {
                    if (authors.All(a => a.AuthorGuid != item.AuthorGuid))
                    {
                        authors.Add(item);
                    }
                }
            }

            List<SelectListItem> result = new List<SelectListItem>();
            
            foreach (var author in authors)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{author.Surname} {author.Name}",
                    Value = author.AuthorGuid.ToString()
                };
                result.Add(item);
            }

            result = result.Distinct().ToList();

            return PartialView(result);
        }

        [HttpGet("twoparams")]
        public async Task<PartialViewResult> TwoParams(string author, string technology)
        {
            var result = await _searchService.SearchOnTwoParameters(
                Guid.Parse(author),
                Guid.Parse(technology));

            var authorModel = await _authorsService.Read(Guid.Parse(author));
            var technologiesModel = await _technologiesService.Read(Guid.Parse(technology));
            
            ViewBag.authorName = $"{authorModel.Surname} {authorModel.Name} {authorModel.Patronymic}";
            ViewBag.authorInfo = $"{authorModel.Note}";
            ViewBag.technology = $"{technologiesModel.Name}";
            
            return PartialView(result);
        }

        [HttpGet("authorssearch")]
        public async Task<PartialViewResult> AuthorsSearch(string authorInput)
        {
            var authors = await _searchService.SearchOnAuthor(authorInput);
            var allBooks = await _booksService.ReadAll();

            var books = new List<BookModel>();
            
            foreach (var author in authors)
            {
                var book = allBooks.Where(a => a.AuthorGuid.Contains(author.AuthorGuid)).ToList();
                books.AddRange(book);
            }

            books = books.Distinct().ToList();

            ViewBag.search = authorInput;
            
            var result = new SearchOnAuthor()
            {
                Authors = authors,
                Books = books,
            };

            return PartialView(result);
        }

        [HttpGet("publishingssearch")]
        public async Task<PartialViewResult> PublishingsSearch(string pubInput)
        {
            var publishings = await _searchService.SearchOnPublishing(pubInput);
            var allBooks = await _booksService.ReadAll();

            var books = new List<BookModel>();
            
            foreach (var publishing in publishings)
            {
                var book = allBooks.Where(a => a.PublishingGuid.Equals(publishing.PublishingGuid)).ToList();
                books.AddRange(book);
            }

            books = books.Distinct().ToList();

            ViewBag.search = pubInput;
            
            var result = new SearchOnPublishing()
            {
                Publishings = publishings,
                Books = books,
            };

            return PartialView(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> SearchAll(string param)
        {
            var result = await _globalSearchService.Search(param);

            return View(result);
        }
    }
}