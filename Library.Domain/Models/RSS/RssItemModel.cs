using Library.Database.Models.RSS;

namespace Library.Domain.Models.RSS
{
    public class RssItemModel : RssItem
    {
        public string PubDateString { get; set; }
    }
}