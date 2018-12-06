using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Database.Models
{
    /// <summary>
    /// Сфера применения
    /// </summary>
    public class Technology
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid TechnologyGuid { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Языки программирования
        /// </summary>
        public string Language { get; set; }
    }
}