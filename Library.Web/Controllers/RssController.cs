using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models.RSS;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Route("rss")]
    public class RssController : Controller
    {
        private readonly IRssService _rssService;

        public RssController(IRssService rssService)
        {
            _rssService = rssService;
        }

        [HttpGet]
        public async Task<ViewResult> GetAll()
        {
            await _rssService.Update();
            var results = await _rssService.GetAllGroups();
            return View(results);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddSource(string uri)
        {
            await _rssService.AddSource(uri);
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet("update")]
        public async Task<RedirectToActionResult> Update()
        {
            await _rssService.Update();
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpPost("create")]
        public async Task<RedirectToActionResult> AddMySource(string name)
        {
            await _rssService.AddMySource(name);
            return RedirectToAction(nameof(GetAll));
        }

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