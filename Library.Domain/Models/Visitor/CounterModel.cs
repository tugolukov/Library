using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library.Domain.Models.Visitor
{
    public class CounterModel
    {
        public int LastDayVisits { get; set; }
        public int TotalVisits { get; set; }
        public int SiteVisits { get; set; }
    }
}