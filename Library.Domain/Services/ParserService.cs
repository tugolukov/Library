using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;
using Library.Parser.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class ParserService : IParserService
    {
        private readonly IPiterParser _piterParser;
        private readonly IEksmoParser _eksmoParser;
        private readonly IAuthorsService _authorsService;
        private readonly IPublishingsService _publishingsService;
        private readonly ITechnologiesService _technologiesService;
        private readonly IBooksService _booksService;

        /// <inheritdoc />
        public ParserService(
            IPiterParser piterParser,
            IAuthorsService authorsService,
            IPublishingsService publishingsService,
            ITechnologiesService technologiesService,
            IBooksService booksService, IEksmoParser eksmoParser)
        {
            _piterParser = piterParser;
            _authorsService = authorsService;
            _publishingsService = publishingsService;
            _technologiesService = technologiesService;
            _booksService = booksService;
            _eksmoParser = eksmoParser;
        }

        /// <inheritdoc />
        public async Task ParsePiter()
        {
            var books = _piterParser.GetPiter();


            #region Создание/Получение издательства

            Guid publishingGuid = Guid.Empty;

            CreatePublishingModel publishing = new CreatePublishingModel();
            publishing.PublishingGuid = Guid.NewGuid();
            publishing.Name = "Издательский дом 'Питер'";
            publishing.City = "г. Санкт-Петербург";
            publishing.Country = "Россия";
            publishing.House = "дом № 29, литера А";
            publishing.Postcode = 194044;
            publishing.State = "Ленинградская обл.";
            publishing.Street = "Большой Сампсониевский пр-кт";

            var searchPublishing = await _publishingsService.Search(publishing.Name);
            if (searchPublishing.Any())
            {
                var publishingModel = await _publishingsService.Read(searchPublishing.First().PublishingGuid);
                publishingGuid = publishingModel.PublishingGuid;
            }
            else
            {
                publishingGuid = await _publishingsService.Create(publishing);
            }

            #endregion

            foreach (var book in books)
            {
                try
                {
                    CreateBookModel createBookModel = new CreateBookModel();

                    // Издательство и авторы
                    createBookModel.CreationDateTimeOffset = DateTimeOffset.Now;
                    createBookModel.PublishingGuid = publishingGuid;
                    createBookModel.AuthorGuid = new List<Guid>();

                    #region Создание/Добавление автора

                    foreach (var authorsName in book.AuthorsNames)
                    {
                        var fullname = authorsName.Split(' ');
                        CreateAuthorModel author = new CreateAuthorModel();
                        author.AuthorGuid = Guid.NewGuid();
                        author.Surname = fullname[0];
                        author.Name = fullname[1];
                        if (fullname.Length == 3)
                        {
                            author.Patronymic = fullname[2];
                        }

                        author.Note = "Автор книг: \"" + book.Title + "\"";

                        var searchAuthor = await _authorsService.Search(author.Surname);
                        if (searchAuthor.Any())
                        {
                            foreach (var item in searchAuthor)
                            {
                                UpdateAuthorModel updateAuthorModel = new UpdateAuthorModel();
                                updateAuthorModel.AuthorGuid = item.AuthorGuid;
                                updateAuthorModel.Name = item.Name;
                                updateAuthorModel.Surname = item.Surname;
                                updateAuthorModel.Patronymic = item.Patronymic;
                                if (!item.Note.Contains(book.Title))
                                {
                                    updateAuthorModel.Note = item.Note + ", \"" + book.Title + "\"";
                                }

                                await _authorsService.Update(updateAuthorModel);
                                createBookModel.AuthorGuid.Add(item.AuthorGuid);
                            }
                        }
                        else
                        {
                            var guid = await _authorsService.Create(author);

                            createBookModel.AuthorGuid.Add(guid);
                        }
                    }

                    #endregion

                    #region Создание/Добавление сферы применения

                    // Сфера применения
                    CreateTechnologyModel technology = new CreateTechnologyModel();
                    technology.TechnologyGuid = Guid.NewGuid();
                    technology.Name = book.TechnologiesName;
                    technology.Description = "Серия книг: " + technology.Name;
                    technology.Language = "-";

                    var searchTechnologies = await _technologiesService.Search(technology.Name);
                    if (searchTechnologies.Any())
                    {
                        var technologyModel =
                            await _technologiesService.Read(searchTechnologies.First().TechnologyGuid);
                        createBookModel.TechnologyGuid = technologyModel.TechnologyGuid;
                    }
                    else
                    {
                        createBookModel.TechnologyGuid = await _technologiesService.Create(technology);
                    }

                    #endregion

                    createBookModel.BookGuid = Guid.NewGuid();
                    createBookModel.Title = book.Title;
                    createBookModel.Annotation = book.Annotation;
                    createBookModel.Cover = book.Cover;
                    try
                    {
                        createBookModel.NumberOfPages = Convert.ToInt32(book.NumberOfPages);
                    }
                    catch (Exception e)
                    {
                        createBookModel.NumberOfPages = 0;
                    }

                    createBookModel.Format = book.Format;
                    createBookModel.Cost =
                        "Электронная книга: " + book.CostDigital + ";\nБумажная книга: " + book.CostPaper;

                    try
                    {
                        createBookModel.Year = Convert.ToInt32(book.Year);
                    }
                    catch (Exception e)
                    {
                        createBookModel.Year = 2019;
                    }

                    createBookModel.BuyUri = book.BuyUri;
                    createBookModel.ImageUri = book.ImageUri;

                    var result = await _booksService.Create(createBookModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public Task ParseEksmo()
        {
            _eksmoParser.GetEksmo();
            return null;
        }
    }
}