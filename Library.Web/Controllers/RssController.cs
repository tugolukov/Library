using System;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.RSS;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    /// <summary>
    /// RSS-контроллер
    /// </summary>
    [Route("rss")]
    public class RssController : Controller
    {
        private readonly IRssService _rssService;

        public RssController(IRssService rssService)
        {
            _rssService = rssService;
        }

        /// <summary>
        /// Получение страницы с новостями
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ViewResult> GetAll()
        {
            var results = await _rssService.GetSources();
            return View(results);
        }

        /// <summary>
        /// Добавить источник из URL
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RedirectToActionResult> AddSource(string uri)
        {
            await _rssService.AddSourceWithUrl(uri);
            return RedirectToAction(nameof(GetAll));
        }
        
        /// <summary>
        /// Создать свой канал
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<RedirectToActionResult> AddMySource(string name)
        {
            await _rssService.AddSourceWithoutUrl(name);
            return RedirectToAction(nameof(GetAll));
        }

        /// <summary>
        /// Добавить новость
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("article")]
        public async Task<RedirectToActionResult> AddArticle(RssItemModel item)
        {
            item.RssItemGuid = Guid.NewGuid();
            item.PubDate = DateTimeOffset.Now;
            var result = await _rssService.AddItem(item);
            return RedirectToAction(nameof(GetAll));
        }
        
    }
}