using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;

namespace Library.Database.Models
{
    public class Book
    {
        /// <summary>
        /// Идентификатор книги
        /// </summary>
        [Key]
        public Guid BookGuid { get; set; }

        #region FK

        /// <summary>
        /// Идентификатор автора
        /// </summary>
        public List<Guid> AuthorGuid { get; set; }

        /// <summary>
        /// Идентификатор издательства
        /// </summary>
        [ForeignKey(nameof(Publishing))]
        public Guid PublishingGuid { get; set; }

        /// <summary>
        /// Идентификатор технологии
        /// </summary>
        [ForeignKey(nameof(Technology))]
        public Guid TechnologyGuid { get; set; }

        #endregion

        /// <summary>
        /// Название
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Аннотация
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// Тип обложки
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Формат книги
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public string Cost { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Ссылка для покупки
        /// </summary>
        public string BuyUri { get; set; }
        
        /// <summary>
        /// Ссылка на картинку обложки
        /// </summary>
        public string ImageUri { get; set; }
        
        /// <summary>
        /// Дата создания в БД
        /// </summary>
        public DateTimeOffset CreationDateTimeOffset { get; set; }

        /// <summary>
        /// Автор книги
        /// </summary>
        public virtual Author Author { get; set; }

        /// <summary>
        /// Издательство
        /// </summary>
        public virtual Publishing Publishing { get; set; }

        /// <summary>
        /// Технология
        /// </summary>
        public virtual Technology Technology { get; set; }


        /// <inheritdoc />
        public Book()
        {
            BookGuid = Guid.NewGuid();
        }
    }
}