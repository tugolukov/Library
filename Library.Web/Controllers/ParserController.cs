using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    /// <summary>
    /// API для парсинга
    /// </summary>
    [Route("/parser")]
    public class ParserController : Controller
    {
        private readonly IParserService _parserService;

        public ParserController(IParserService parserService)
        {
            _parserService = parserService;
        }

        /// <summary>
        /// Получение книг с сайта издательства Питер
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task GetFullPiter()
        {
            await _parserService.ParsePiter();
        }

        [HttpGet("eksmo")]
        public async Task GetEksmo()
        {
            await _parserService.ParseEksmo();
        }
    }
}