using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;

namespace Library.Domain.Interfaces
{
    /// <summary>
    /// Сервис поиска
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Поиск автора и его книг
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<AuthorModel>> SearchOnAuthor(string search);
        
        /// <summary>
        /// Поиск издательства и его книг
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<PublishingModel>> SearchOnPublishing(string search);
        
        /// <summary>
        /// Поиск по двум параметрам
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <param name="technologyGuid"></param>
        /// <returns></returns>
        Task<List<BookModel>> SearchOnTwoParameters(Guid authorGuid, Guid technologyGuid);
        
        /// <summary>
        /// Поиск везде
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<BookModel>> Search(string search);
    }
}