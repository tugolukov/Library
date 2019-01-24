using System;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Web.Utils.SitemapGenerator;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Route("utils")]
    public class SitemapController : Controller
    {
        private readonly IBooksService _booksService;

        public SitemapController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [Route("sitemap.xml")]
        [HttpGet]
        public async Task<IActionResult> Sitemap()
        {
            string baseUrl = "http://itlibrary.site/books/";

            // get a list of published articles
            var books = await _booksService.ReadAll();

            // get last modified date of the home page
            var siteMapBuilder = new SitemapBuilder();

            // add the home page to the sitemap
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            // add the blog posts to the sitemap
            foreach(var book in books)
            {
                siteMapBuilder.AddUrl(baseUrl + book.BookGuid, modified: DateTime.Now, changeFrequency: null, priority: 0.9);
            }

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }
    }
}