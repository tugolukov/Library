using System.Collections.Generic;

namespace Library.Parser
{
    public class ParserBookModel
    {
        public string Title { get; set; }
        public string Annotation { get; set; }
        public string Cover { get; set; }
        public string NumberOfPages { get; set; }
        public string Format { get; set; }
        public string CostPaper { get; set; }
        public string CostDigital { get; set; }
        public string Year { get; set; }
        public string BuyUri { get; set; }
        public string ImageUri { get; set; }
        public List<string> AuthorsNames { get; set; }
        public string TechnologiesName { get; set; }
        public string PublishingName { get; set; }
    }
}