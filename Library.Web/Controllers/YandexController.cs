using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    public class YandexController : Controller
    {
        // GET
        [Route("yasearch")]
        public IActionResult Index()
        {
            return View();
        }
    }
}