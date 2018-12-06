using System.Collections.Generic;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;

namespace Library.Web.Models.Search
{
    public class SearchOnAuthor
    {
        public List<AuthorModel> Authors { get; set; }
        public List<BookModel> Books { get; set; }
    }
}