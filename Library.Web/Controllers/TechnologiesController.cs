using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Technology;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    /// <summary>
    /// Категории применения
    /// </summary>
    [Route("/technologies")]
    public class TechnologiesController : Controller
    {
        private readonly ITechnologiesService _technologiesService;

        public TechnologiesController(ITechnologiesService technologiesService)
        {
            _technologiesService = technologiesService;
        }

        /// <summary>
        /// Категории применения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            var result = await _technologiesService.ReadAll();
            return View(result);
        }

        [HttpPost]
        public async Task CreateTechnology(CreateTechnologyModel technology)
        {
            technology.Name = technology.TechnologiesName;
            await _technologiesService.Create(technology);
        }
    }
}