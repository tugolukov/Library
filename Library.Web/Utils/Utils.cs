using System.Collections.Generic;
using Library.Domain.Models.Author;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Utils
{
    public static class Utils
    {
        public static List<SelectListItem> ToSelectListItems(this List<AuthorModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var author in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{author.Surname} {author.Name}",
                    Value = author.AuthorGuid.ToString()
                };
                result.Add(item);
            }
            return result;
        }
        
        public static List<SelectListItem> ToSelectListItems(this List<TechnologyModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var technology in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{technology.Name}",
                    Value = technology.TechnologyGuid.ToString()
                };
                result.Add(item);
            }
            return result;
        }
        
        public static List<SelectListItem> ToSelectListItems(this List<PublishingModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var publishing in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{publishing.Name}",
                    Value = publishing.PublishingGuid.ToString()
                };
                result.Add(item);
            }
            return result;
        }
    }
}