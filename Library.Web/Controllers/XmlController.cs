using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class XmlController : Controller
    {
        private readonly IBooksService _booksService;

        public XmlController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("/xml")]
        public async Task<IActionResult> Index()
        {
            var result = await _booksService.ReadAll();
            return View(result);
        }
        
        [HttpGet("/download")]
        public async Task<FileResult> GetXml()
        {
            var path = await GenerateXML();
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/xml", "books.xml");
        }

        private async Task<string> GenerateXML()
        {
            var books = await _booksService.ReadAll(); 
            
            var xElement = new XElement("Books",
                from book in books
                select new XElement("Book",
                    new XElement("Title", book.Title),
                    new XElement("Annotation", book.Annotation),
                    new XElement("Year", book.Year)
                ));

            var path = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(path, "getXML");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var fileName = Guid.NewGuid().ToString() + ".xml";
            var filePath = Path.Combine(fullPath, fileName);

            using (TextWriter tw = System.IO.File.CreateText(filePath))
            {
                await xElement.SaveAsync(tw, SaveOptions.None, CancellationToken.None);
            }
            
            return filePath;
        }
    }
}