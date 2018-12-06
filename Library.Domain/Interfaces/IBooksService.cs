using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Book;

namespace Library.Domain.Interfaces
{
    /// <summary>
    /// Сервис для работы с книгами
    /// </summary>
    public interface IBooksService
    {
        /// <summary>
        /// Создание книги
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Create(CreateBookModel model);

        /// <summary>
        /// Парсинг информации о книге из URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns></returns>
        Task<Guid> CreateFromUri(string uri);

        Task<List<BookModel>> ReadAll();
        
        /// <summary>
        /// Получить список книг
        /// </summary>
        /// <returns></returns>
        Task<BooksList> ReadPage(int page);

        /// <summary>
        /// Получить книгу по идентификатору
        /// </summary>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
        Task<BookModel> Read(Guid bookGuid);

        /// <summary>
        /// Редактировать книгу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdateBookModel model);

        /// <summary>
        /// Удалить книгу по идентификатору
        /// </summary>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
        Task Delete(Guid bookGuid);

        /// <summary>
        /// Поиск книги
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<BookModel>> Search(string search);

        /// <summary>
        /// Вывод статистики по книгам
        /// </summary>
        /// <returns></returns>
        Task<List<StatisticModel>> Statistic();
    }
}