using System;

namespace Library.Domain.Models.Author
{
    /// <inheritdoc />
    public class CreateAuthorModel : Database.Models.Author
    {
        public CreateAuthorModel()
        {
            Name = AuthorName;
        }

        public string AuthorName { get; set; }
    }
}