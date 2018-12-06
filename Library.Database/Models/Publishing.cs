using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Database.Models
{
    /// <summary>
    /// Издательство
    /// </summary>
    public class Publishing
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid PublishingGuid { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }
        
        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// Штат/область
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Индекс
        /// </summary>
        public long Postcode { get; set; }
        
        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; set; }
    }
}