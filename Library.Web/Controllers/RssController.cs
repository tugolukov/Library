using System;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.RSS;
using Library.Web.Models;
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

        [HttpGet]
        public async Task<ViewResult> Get(int page = 1)
        {
            var items = await _rssService.Get();

            RssModel result = new RssModel()
            {
                Items = items.Skip(10*(page-1)).Take(10).ToList(),
                FullItems = items,
                CurrentPage = page,
                PreviousPage = page-1,
                NextPage = page+1,
                Count = items.Count/10 + 1
            };
            
            return View(result);
        }

        /// <summary>
        /// Получение страницы с новостями
        /// </summary>
        /// <returns></returns>
        [HttpGet("/morenews")]
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
            return RedirectToAction(nameof(Get));
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
            return RedirectToAction(nameof(Get));
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
            return RedirectToAction(nameof(Get));
        }
        
    }
}