using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.Web.Controllers
{
    /// <summary>
    /// Работа с авторами
    /// </summary>
    [Route("/authors")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpPost]
        public async Task CreateAuthor([FromForm]CreateAuthorModel author)
        {
            author.Name = author.AuthorName;
            await _authorsService.Create(author);
        }
    }
}