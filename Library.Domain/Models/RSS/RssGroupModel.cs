using System.Collections.Generic;

namespace Library.Domain.Models.RSS
{
    public class RssGroupModel
    {
        public RssSourceModel Source { get; set; }
        
        public List<RssItemModel> Items { get; set; }
    }
}