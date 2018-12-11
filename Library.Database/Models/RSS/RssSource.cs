using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Database.Models.RSS
{
    /// <summary>
    /// Источник RSS
    /// </summary>
    public class RssSource
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid RssSourceGuid { get; set; }

        /// <summary>
        /// Наименование ленты
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Адрес
        /// </summary>
        public string Uri { get; set; }
        
        public RssSource(){}

        public RssSource(string uri, string title)
        {
            RssSourceGuid = Guid.NewGuid();
            Title = title;
            Uri = uri;
        }

        public RssSource(string title)
        {
            RssSourceGuid = Guid.NewGuid();
            Title = title;
            Uri = null;
        }
        
    }
}