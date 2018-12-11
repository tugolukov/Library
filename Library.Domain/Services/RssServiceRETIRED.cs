using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Library.Database;
using Library.Database.Models.RSS;
using Library.Domain.Interfaces;
using Library.Domain.Models.RSS;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class RssServiceRETIRED : IRssService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public RssServiceRETIRED(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<RssGroupModel>> GetSources()
        {
            var sources = await _context.RssSources.ToListAsync();
            List<RssGroupModel> results = new List<RssGroupModel>();
            
            foreach (var source in sources)
            {
                var guid = source.RssSourceGuid;
                List<RssItemModel> items = new List<RssItemModel>();
                if (source.Uri != null)
                {
                    items = await GetFromUri(source.Uri);
                }
                else
                {
                    var itemsModel = await _context.RssItems.Where(a => a.RssSourceGuid == source.RssSourceGuid).ToListAsync();
                    items = _mapper.Map<List<RssItem>, List<RssItemModel>>(itemsModel);
                }
                var group = new RssGroupModel()
                {
                    Source = _mapper.Map<RssSourceModel>(source),
                    Items = items.Take(15).ToList()
                };
                
                results.Add(group);
            }

            return results;
        }

        /// <inheritdoc />
        public async Task<List<RssItemModel>> GetAll()
        {
            var items = await _context.RssItems.ToListAsync();
            return _mapper.Map<List<RssItem>, List<RssItemModel>>(items);        
        }

        /// <inheritdoc />
        public async Task<Guid> AddSourceWithUrl(string uri)
        {
            RssSource source = new RssSourceModel()
            {
                RssSourceGuid = Guid.NewGuid(),
                Title = GetTitleFromUri(uri),
                Uri = uri
            };
            XDocument doc = new XDocument(); 

            try
            {
                doc = XDocument.Load(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            _context.RssSources.Add(source);
            await _context.SaveChangesAsync();
                
            AddNewItems(uri, doc);
            
            return source.RssSourceGuid;
        }

        public async Task<Guid> AddSourceWithoutUrl(string name)
        {
            RssSource source = new RssSourceModel()
            {
                RssSourceGuid = Guid.NewGuid(),
                Title = name,
                Uri = null
            };
            
            _context.RssSources.Add(source);
            await _context.SaveChangesAsync();

            return source.RssSourceGuid;
        }

        public async Task<Guid> AddItem(RssItemModel item)
        {
            RssItem rssItem = _mapper.Map<RssItemModel>(item);
            _context.RssItems.Add(rssItem);
            await _context.SaveChangesAsync();
            return item.RssItemGuid;
        }

        /// <inheritdoc />
        public async Task Update()
        {
            var sources = await _context.RssSources.ToListAsync();
            foreach (var source in sources)
            {
                try
                {
                    await AddNewItems(source.Uri, XDocument.Load(source.Uri));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private string GetTitleFromUri(string uri)
        {
            try
            {
                XDocument doc = XDocument.Load(uri);
                return doc.Root.Descendants("channel").Elements("title").FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        private async Task<List<RssItemModel>> GetFromUri(string uri)
        {
            try
            {
                var source = await _context.RssSources.SingleOrDefaultAsync(a => a.Uri == uri);
                var items = await _context.RssItems.Where(a => a.RssSourceGuid == source.RssSourceGuid).OrderByDescending(a => a.PubDate).ToListAsync();
                
                return _mapper.Map<List<RssItem>, List<RssItemModel>>(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private async Task AddNewItems(string uri, XDocument doc)
        {
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var source = await _context.RssSources.SingleOrDefaultAsync(a => a.Uri == uri);
            if (source == null)
            {
                RssSourceModel sourceModel = new RssSourceModel()
                {
                    RssSourceGuid = Guid.NewGuid(),
                    Uri = uri
                };
                _context.RssSources.Add(sourceModel);
                await _context.SaveChangesAsync();
                source = sourceModel;
            }
            
            var root = doc.Root.Descendants("item");
            foreach (var item in root)
            {
                RssItemModel itemModel = new RssItemModel()
                {
                    RssItemGuid = Guid.NewGuid(),
                    RssSourceGuid = source.RssSourceGuid,
                    Description = item.Element("description").Value,
                    Link = item.Element("link").Value,
                    PubDate = Convert.ToDateTime(item.Element("pubDate").Value),
                    Title = item.Element("title").Value
                };
                
                try
                {
                    itemModel.Enclosure = item.Element("enclosure").Attribute("url").Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                var kek = await _context.RssItems.Where(a => a.Title.ToUpper().Equals(itemModel.Title.ToUpper())).ToListAsync();

                if (kek.Count == 0)
                {
                    _context.RssItems.Add(itemModel);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}