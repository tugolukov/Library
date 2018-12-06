using System.Collections.Generic;
using Library.Domain.Models.Author;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;

namespace Library.Domain.Models.Book
{
    /// <inheritdoc />
    public class BookModel : Database.Models.Book
    {
        /// <summary>
        /// Модель автора
        /// </summary>
        public List<AuthorModel> AuthorModel { get; set; }
        
        /// <summary>
        /// Модель издательства
        /// </summary>
        public PublishingModel PublishingModel { get; set; }
        
        /// <summary>
        /// Модель сферы применения
        /// </summary>
        public TechnologyModel TechnologyModel { get; set; }
    }
}