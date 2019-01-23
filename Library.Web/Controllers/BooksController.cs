using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;
using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    /// <summary>
    /// Работа с книгами
    /// </summary>
    [Route("books")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly ITechnologiesService _technologiesService;
        private readonly IAuthorsService _authorsService;
        private readonly IPublishingsService _publishingsService;

        
        public BooksController(
            IBooksService booksService, 
            IPublishingsService publishingsService, 
            IAuthorsService authorsService, 
            ITechnologiesService technologiesService)
        {
            _booksService = booksService;
            _publishingsService = publishingsService;
            _authorsService = authorsService;
            _technologiesService = technologiesService;
        }

        /// <summary>
        /// Просмотр книг
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ReadAll(int page = 1)
        {
            var result = await _booksService.ReadPage(page);
            ViewBag.description = "книги программирование авторы список";
            return View(result);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Read([FromRoute] Guid guid)
        {
            var result = await _booksService.Read(guid);
            ViewBag.description = result.GetDescriptionTags();
            return View(result);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var authors = await _authorsService.ReadAll();
            var publishings = await _publishingsService.ReadAll();
            var technologies = await _technologiesService.ReadAll();

            authors.Sort(delegate(AuthorModel model, AuthorModel authorModel)
                {
                    return string.Compare(model.Surname, authorModel.Surname, StringComparison.Ordinal);
                });
            
            publishings.Sort(delegate(PublishingModel model, PublishingModel publishingModel)
                {
                    return string.Compare(model.Name, publishingModel.Name, StringComparison.Ordinal);
                });
            
            technologies.Sort(delegate(TechnologyModel model, TechnologyModel technologyModel)
                {
                    return string.Compare(model.Name, technologyModel.Name, StringComparison.Ordinal);
                });
            
            ViewBag.authors = authors.ToSelectListItems();
            ViewBag.publishings = publishings.ToSelectListItems();
            ViewBag.technologies = technologies.ToSelectListItems();
            
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBook(CreateBookModel bookModel)
        {
            await _booksService.Create(bookModel);
            return RedirectToAction(nameof(ReadAll));
        }

        [HttpGet("edit/{guid}")]
        public async Task<IActionResult> Edit([FromRoute] Guid guid)
        {
            var book = await _booksService.Read(guid);
            
            var authors = await _authorsService.ReadAll();
            var publishings = await _publishingsService.ReadAll();
            var technologies = await _technologiesService.ReadAll();

            authors.Sort(delegate(AuthorModel model, AuthorModel authorModel)
            {
                return string.Compare(model.Surname, authorModel.Surname, StringComparison.Ordinal);
            });
            
            publishings.Sort(delegate(PublishingModel model, PublishingModel publishingModel)
            {
                return string.Compare(model.Name, publishingModel.Name, StringComparison.Ordinal);
            });
            
            technologies.Sort(delegate(TechnologyModel model, TechnologyModel technologyModel)
            {
                return string.Compare(model.Name, technologyModel.Name, StringComparison.Ordinal);
            });


            ViewBag.authors = authors.ToSelectListItems();
            ViewBag.publishings = publishings.ToSelectListItems();
            ViewBag.technologies = technologies.ToSelectListItems();
            
            return View(book);
        }

        [HttpPost("edit/{guid}")]
        public async Task<RedirectToActionResult> EditBook([FromRoute]Guid guid, [FromForm]UpdateBookModel bookModel)
        {
            bookModel.BookGuid = guid;
            await _booksService.Update(bookModel);
            return RedirectToAction(nameof(ReadAll));
        }


        [HttpPost]
        public async Task<PartialViewResult> CreateAuthor()
        {
            var authors = await _authorsService.ReadAll();

            authors.Sort(delegate(AuthorModel model, AuthorModel authorModel)
            {
                return string.Compare(model.Surname, authorModel.Surname, StringComparison.Ordinal);
            });

            return PartialView(authors.ToSelectListItems()); 
        }

        [HttpGet("delete")]
        public async Task<RedirectToActionResult> Delete(Guid guid)
        {
            await _booksService.Delete(guid);
            return RedirectToAction(nameof(ReadAll));
        }

        [HttpGet("statistic")]
        public async Task<IActionResult> Statistic()
        {
            var result = await _booksService.Statistic();
            return View(result);
        }
        
        [HttpGet("authors")]
        public async Task<IActionResult> GetAuthors()
        {
            await Task.Delay(1000);
            var authors = await _authorsService.ReadAll();

            authors.Sort(delegate(AuthorModel model, AuthorModel authorModel)
            {
                return string.Compare(model.Surname, authorModel.Surname, StringComparison.Ordinal);
            });

            return PartialView(authors.ToSelectListItems());
        }
        
        [HttpGet("publishings")]
        public async Task<IActionResult> GetPublishings()
        {
            await Task.Delay(1000);
            var publishings = await _publishingsService.ReadAll();

            publishings.Sort(delegate(PublishingModel model, PublishingModel publishingModel)
            {
                return string.Compare(model.Name, publishingModel.Name, StringComparison.Ordinal);
            });

            return PartialView(publishings.ToSelectListItems());
        }
        
        [HttpGet("technologies")]
        public async Task<IActionResult> GetTechnologies()
        {
            await Task.Delay(1000);
            var technologies = await _technologiesService.ReadAll();

            technologies.Sort(delegate(TechnologyModel model, TechnologyModel technologyModel)
            {
                return string.Compare(model.Name, technologyModel.Name, StringComparison.Ordinal);
            });
            
            return PartialView(technologies.ToSelectListItems());
        }
    }
}