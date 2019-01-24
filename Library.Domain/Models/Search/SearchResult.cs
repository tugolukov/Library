namespace Library.Domain.Models.Search
{
    public class SearchResult
    {
        public string Uri { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public double Probability { get; set; }
    }
}