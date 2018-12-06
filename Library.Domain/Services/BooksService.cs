using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Database;
using Library.Database.Models;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class BooksService : IBooksService
    {       
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        
        private readonly IAuthorsService _authorsService;
        private readonly IPublishingsService _publishingsService;
        private readonly ITechnologiesService _technologiesService;

        /// <inheritdoc />
        public BooksService(
            DatabaseContext context, 
            IMapper mapper,
            IAuthorsService authorsService,
            IPublishingsService publishingsService,
            ITechnologiesService technologiesService)
        {
            _context = context;
            _mapper = mapper;
            
            _authorsService = authorsService;
            _publishingsService = publishingsService;
            _technologiesService = technologiesService;
        }

        /// <inheritdoc />
        public async Task<Guid> Create(CreateBookModel model)
        {
            model.CreationDateTimeOffset = DateTimeOffset.Now;
            var book = _mapper.Map<Book>(model);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book.BookGuid;
        }

        /// <inheritdoc />
        public Task<Guid> CreateFromUri(string uri)
        {
            // TODO: Добавить выбор сервиса парсинга и отобразить результат(возможно, вернуть не Guid, а объект с результатом)
            throw new NotImplementedException();
        }

        public async Task<List<BookModel>> ReadAll()
        {
            var books = await _context.Books.OrderByDescending(a => a.CreationDateTimeOffset).ToListAsync();
            List<BookModel> results = new List<BookModel>();
            foreach (var book in books)
            {
                var bookModel = await Read(book.BookGuid);
                results.Add(bookModel);
            }

            return results;
        }

        /// <inheritdoc />
        public async Task<BooksList> ReadPage(int page = 1)
        {
            int pageSize = 10;
            
            var books = await _context.Books.OrderByDescending(a => a.CreationDateTimeOffset).ToListAsync();
            int totalCount = books.Count;
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            List<BookModel> results = new List<BookModel>();
            foreach (var book in books)
            {
                var bookModel = await Read(book.BookGuid);
                results.Add(bookModel);
            }

            double count = totalCount / pageSize;
            
            return new BooksList()
            {
                Books = results,
                TotalCount = totalCount,
                PreviousPage = page - 1,
                CurrentPage = page,
                NextPage = page + 1,
                Count = Math.Ceiling(count)
            };
        }

        /// <inheritdoc />
        public async Task<BookModel> Read(Guid bookGuid)
        {
            var book = await _context.Books.FindAsync(bookGuid);
            var bookModel = _mapper.Map<BookModel>(book);
            bookModel.AuthorModel = new List<AuthorModel>();
            foreach (var guid in book.AuthorGuid)
            {
                var author = await _authorsService.Read(guid);
                bookModel.AuthorModel.Add(author);
            }
            bookModel.PublishingModel = await _publishingsService.Read(book.PublishingGuid);
            bookModel.TechnologyModel = await _technologiesService.Read(book.TechnologyGuid);

            return bookModel;
        }

        /// <inheritdoc />
        public async Task Update(UpdateBookModel model)
        {
            var book = await _context.Books.FindAsync(model.BookGuid);
            book.BuyUri = model.BuyUri;
            book.Annotation = model.Annotation;
            book.Cost = model.Cost;
            book.Cover = model.Cover;
            book.Format = model.Format;
            book.Title = model.Title;
            book.Year = model.Year;
            book.AuthorGuid = model.AuthorGuid;
            book.BookGuid = model.BookGuid;
            book.ImageUri = model.ImageUri;
            book.PublishingGuid = model.PublishingGuid;
            book.TechnologyGuid = model.TechnologyGuid;
            book.NumberOfPages = model.NumberOfPages;
            book.CreationDateTimeOffset = DateTimeOffset.Now;

            _context.Books.Update(book);

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid bookGuid)
        {
            var book = await _context.Books.FindAsync(bookGuid);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<BookModel>> Search(string search)
        {
            var books = await _context.Books
                .Where(book => book.Title.Contains(search) ||
                               book.Annotation.Contains(search)).ToListAsync();
            
            return _mapper.Map<List<Book>, List<BookModel>>(books);        
        }

        public async Task<List<StatisticModel>> Statistic()
        {
            var technologies = await _technologiesService.ReadAll();
            List<StatisticModel> result = new List<StatisticModel>();
            foreach (var technology in technologies)
            {
                result.Add(new StatisticModel()
                {
                    Count = _context.Books.Count(a => a.TechnologyGuid == technology.TechnologyGuid),
                    Technology = technology
                });
            }
            return result;
        }
    }
}