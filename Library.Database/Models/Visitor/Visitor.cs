using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Database.Models.Visitor
{
    public class Visitor
    {
        [Key]
        public Guid VisitorGuid { get; set; }
        public string HostAdress { get; set; }
        public string UserAgent { get; set; }
        public string PageUrl { get; set; }
        public DateTimeOffset VisitTime { get; set; }
    }
}