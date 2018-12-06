using Library.Domain.Models.Technology;

namespace Library.Domain.Models.Book
{
    public class StatisticModel
    {
        public TechnologyModel Technology { get; set; }
        public int Count { get; set; }
    }
}