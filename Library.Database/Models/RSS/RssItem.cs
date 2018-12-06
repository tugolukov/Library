using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Database.Models.RSS
{
    /// <summary>
    /// Публикация в RSS
    /// </summary>
    public class RssItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid RssItemGuid { get; set; }

        /// <summary>
        /// Идентификатор источника
        /// </summary>
        [ForeignKey(nameof(RssSource))]
        public Guid RssSourceGuid { get; set; }
        
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ссылка
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public DateTimeOffset PubDate { get; set; }

        /// <summary>
        /// Изображение (опционально)
        /// </summary>
        public string Enclosure { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        public virtual RssSource RssSource { get; set; }
    }
}