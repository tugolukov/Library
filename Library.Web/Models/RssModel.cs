using System.Collections.Generic;
using Library.Domain.Models.RSS;

namespace Library.Web.Models
{
    public class RssModel
    {
        public List<RssItemModelFull> Items { get; set; }

        public List<RssItemModelFull> FullItems { get; set; }
        
        public int PreviousPage { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }

        public int Count { get; set; }
    }
}