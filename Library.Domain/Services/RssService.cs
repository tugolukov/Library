using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CodeHollow.FeedReader;
using Library.Database;
using Library.Database.Models.RSS;
using Library.Domain.Interfaces;
using Library.Domain.Models.RSS;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Services
{
    /// <summary>
    /// Сервис для работы с RSS
    /// </summary>
    public class RssService : IRssService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public RssService(DatabaseContext context, IMapper mapper)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);   
            _context = context;
            _mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<List<RssGroupModel>> GetSources()
        {
            await Update();
            List<RssGroupModel> result = new List<RssGroupModel>();
            
            var sources = await _context.RssSources.ToListAsync();
            foreach (var source in sources)
            {
                var group = new RssGroupModel()
                {
                    Items = await GetItems(source.RssSourceGuid),
                    Source = _mapper.Map<RssSourceModel>(source)
                };
                
                result.Add(group);
            }
            
            return result;
        }

        /// <inheritdoc />
        public async Task<Guid> AddSourceWithUrl(string uri)
        {
            return await AddSourceFromUri(uri);
        }

        /// <inheritdoc />
        public async Task<Guid> AddSourceWithoutUrl(string name)
        {
            return await AddEmptySource(name);
        }

        /// <inheritdoc />
        public async Task<Guid> AddItem(RssItemModel item)
        {
            RssItem rssItem = _mapper.Map<RssItemModel>(item);
            _context.RssItems.Add(rssItem);
            await _context.SaveChangesAsync();
            return item.RssItemGuid;        
        }

        /// <summary>
        /// Обновить все источники
        /// </summary>
        /// <returns></returns>
        private async Task Update()
        {
            var sources = await _context.RssSources.Where(a => a.Uri != null).ToListAsync();
            foreach (var source in sources)
            {
                await Update(source);
            }
        }

        /// <summary>
        /// Обновить источник по идентификатору
        /// </summary>
        /// <param name="source">Источник</param>
        /// <returns></returns>
        private async Task Update(RssSource source)
        {
            var feed = await FeedReader.ReadAsync(source.Uri);
            
            await AddItems(source.RssSourceGuid, feed.Items);
        }

        /// <summary>
        /// Получение новостей по идентификатору источника
        /// </summary>
        /// <param name="guid">Идентификатор источника</param>
        /// <returns></returns>
        private async Task<List<RssItemModel>> GetItems(Guid guid)
        {
            var itemsModel = await _context.RssItems.Where(a => a.RssSourceGuid == guid).ToListAsync();
            var items = _mapper.Map<List<RssItem>, List<RssItemModel>>(itemsModel);

            foreach (var item in items)
            {
                if (item.PubDate.Date.Equals(Convert.ToDateTime("01.01.0001 0:00:00")))
                {
                    item.PubDateString = null;
                }
                else if (item.PubDate.Day == DateTime.Today.Day)
                {
                    item.PubDateString = "сегодня в " + item.PubDate.ToString("HH:mm");
                }
                else if (@item.PubDate + TimeSpan.FromDays(1) == DateTime.Today)
                {
                    item.PubDateString = "вчера в " + item.PubDate.ToString("HH:mm");
                }
                else
                {
                    item.PubDateString = item.PubDate.Date.ToShortDateString();
                }
            }

            return items;
        }

        /// <summary>
        /// Добавить пустой источник
        /// </summary>
        /// <param name="title">Название источника</param>
        /// <returns></returns>
        private async Task<Guid> AddEmptySource(string title)
        {
            RssSource source = new RssSource(title);
            _context.RssSources.Add(source);
            await _context.SaveChangesAsync();
            return source.RssSourceGuid;
        }

        /// <summary>
        /// Добавить источник из URL
        /// </summary>
        /// <param name="uri">URL</param>
        /// <returns></returns>
        private async Task<Guid> AddSourceFromUri(string uri)
        {
            var feed = await FeedReader.ReadAsync(uri);
            
            RssSource source = new RssSource(uri, feed.Title);
            _context.RssSources.Add(source);
            await _context.SaveChangesAsync();
            
            await AddItems(source.RssSourceGuid, feed.Items);
            
            return source.RssSourceGuid;
        }

        /// <summary>
        /// Добавление новостей из источника
        /// </summary>
        /// <param name="guid">Идентификатор источника</param>
        /// <param name="items">Новости</param>
        /// <returns></returns>
        private async Task AddItems(Guid guid, ICollection<FeedItem> items)
        {
            foreach (var item in items)
            {               
                RssItem rssItem = new RssItem()
                {
                    Description = item.Description,
                    Enclosure = item.Content,
                    Link = item.Link,
                    RssItemGuid = Guid.NewGuid(),
                    RssSourceGuid = guid,
                    Title = item.Title
                };
                
                if (item.PublishingDateString != null) 
                    rssItem.PubDate = Convert.ToDateTime(item.PublishingDateString);

                var any = await _context.RssItems
                    .Where(a => a.RssSourceGuid == guid)
                    .Where(b => b.Title.Equals(item.Title)).ToListAsync();
                
                if (any.Count == 0)
                {
                    _context.RssItems.Add(rssItem);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}