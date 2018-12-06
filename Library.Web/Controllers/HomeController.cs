using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Library.Web.Models;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Главная страница
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Страница "О разработчиках"
        /// </summary>
        /// <returns></returns>
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Страница "Правила сайта"
        /// </summary>
        /// <returns></returns>
        [HttpGet("rules")]
        public IActionResult Rules()
        {
            return View();
        }

        [HttpGet("links")]
        public IActionResult Links()
        {
            return View();
        }
        
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        [HttpGet("robots.txt")]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();
        
            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("allow: /error/foo");
            stringBuilder.AppendLine("sitemap: http://itlibrary.site/sitemap.xml");
        
            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }
    
        [Route("sitemap.xml")]
        [HttpGet]
        public ContentResult SitemapXml()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string path = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(path, "wwwroot\\files\\sitemap.xml");

            var strings = System.IO.File.ReadAllLines(fullPath);
            foreach (var str in strings)
            {
                stringBuilder.AppendLine(str);
            }

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}