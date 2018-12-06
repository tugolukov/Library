using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.Publishing;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    /// <summary>
    /// Издательства
    /// </summary>
    [Route("/publishings")]
    public class PublishingsController : Controller
    {
        private readonly IPublishingsService _publishingsService;

        public PublishingsController(IPublishingsService publishingsService)
        {
            _publishingsService = publishingsService;
        }

        [HttpPost]
        public async Task CreatePublishing(CreatePublishingModel publishing)
        {
            publishing.Name = publishing.PublishingName;
            await _publishingsService.Create(publishing);
        }
    }
}