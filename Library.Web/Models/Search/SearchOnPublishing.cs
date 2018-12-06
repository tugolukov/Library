using System.Collections.Generic;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;

namespace Library.Web.Models.Search
{
    public class SearchOnPublishing
    {
        public List<PublishingModel> Publishings { get; set; }
        public List<BookModel> Books { get; set; }
    }
}