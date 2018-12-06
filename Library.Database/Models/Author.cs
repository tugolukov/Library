using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Database.Models
{
    /// <summary>
    /// Автор
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Идентификатор автора
        /// </summary>
        [Key]
        public Guid AuthorGuid { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [MaxLength(100)]
        public string Patronymic { get; set; }
        
        /// <summary>
        /// Заметки
        /// </summary>
        public string Note { get; set; }

        /// <inheritdoc />
        public Author()
        {
            AuthorGuid = Guid.NewGuid();
        }
    }
}