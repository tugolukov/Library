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
using Library.Domain.Models.Publishing;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Services
{
    public class SearchService : ISearchService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        private readonly IAuthorsService _authorsService;
        private readonly IBooksService _booksService;
        private readonly IPublishingsService _publishingsService;
        private readonly ITechnologiesService _technologiesService;

        public SearchService(
            DatabaseContext context,
            IMapper mapper,
            IAuthorsService authorsService,
            IBooksService booksService,
            IPublishingsService publishingsService,
            ITechnologiesService technologiesService)
        {
            _context = context;
            _mapper = mapper;

            _authorsService = authorsService;
            _booksService = booksService;
            _publishingsService = publishingsService;
            _technologiesService = technologiesService;
        }

        public async Task<List<AuthorModel>> SearchOnAuthor(string search)
        {
            var authors = await _authorsService.Search(search);
            return authors;
        }

        public async Task<List<PublishingModel>> SearchOnPublishing(string search)
        {
            var publishings = await _publishingsService.Search(search);
            return publishings;
        }

        public async Task<List<BookModel>> SearchOnTwoParameters(Guid authorGuid, Guid technologyGuid)
        {
            var books = new List<Book>();

            if (authorGuid == Guid.Empty & technologyGuid != Guid.Empty)
            {
                books = await _context.Books.Where(b => b.TechnologyGuid == technologyGuid).ToListAsync();
            }

            if (technologyGuid == Guid.Empty & authorGuid != Guid.Empty)
            {
                books = await _context.Books.Where(a => a.AuthorGuid.Contains(authorGuid)).ToListAsync();
            }

            if (authorGuid != Guid.Empty & technologyGuid != Guid.Empty)
            {
                books = await _context.Books.Where(a => a.AuthorGuid.Contains(authorGuid))
                    .Where(b => b.TechnologyGuid == technologyGuid).ToListAsync();
            }


            var result = new List<BookModel>();
            foreach (var book in books)
            {
                var bookModel = await _booksService.Read(book.BookGuid);
                result.Add(bookModel);
            }

            return result;
        }

        public Task<List<BookModel>> Search(string search)
        {
            throw new NotImplementedException();
        }
    }
}