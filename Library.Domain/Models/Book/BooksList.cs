using System.Collections.Generic;

namespace Library.Domain.Models.Book
{
    public class BooksList
    {
        public List<BookModel> Books { get; set; }

        public int TotalCount { get; set; }

        public int PreviousPage { get; set; }
        
        public int CurrentPage { get; set; }

        public int NextPage { get; set; }
        
        public double Count { get; set; }
    }
}