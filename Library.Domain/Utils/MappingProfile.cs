using AutoMapper;
using Library.Database.Models;
using Library.Database.Models.RSS;
using Library.Database.Models.Visitor;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.RSS;
using Library.Domain.Models.Technology;
using Library.Domain.Models.Visitor;

namespace Library.Domain.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorModel>();
            CreateMap<CreateAuthorModel, Author>();
            CreateMap<UpdateAuthorModel, Author>();

            CreateMap<Book, BookModel>();
            CreateMap<CreateBookModel, Book>();
            CreateMap<UpdateBookModel, Book>();
            
            CreateMap<Publishing, PublishingModel>();
            CreateMap<CreatePublishingModel, Publishing>();
            CreateMap<UpdatePublishingModel, Publishing>();
            
            CreateMap<Technology, TechnologyModel>();
            CreateMap<CreateTechnologyModel, Technology>();
            CreateMap<UpdateTechnologyModel, Technology>();

            CreateMap<RssItem, RssItemModel>();
            CreateMap<RssItemModel, RssItem>();

            CreateMap<RssSource, RssSourceModel>();
            CreateMap<RssSourceModel, RssSource>();
            
            CreateMap<Visitor, CreateVisitorModel>();
            CreateMap<CreateVisitorModel, Visitor>();
        }
    }
}