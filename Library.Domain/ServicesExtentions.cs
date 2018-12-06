using Library.Domain.Interfaces;
using Library.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Domain
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddScoped<IAuthorsService, AuthorsService>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IPublishingsService, PublishingsService>();
            services.AddScoped<ITechnologiesService, TechnologiesService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<IRssService, RssService>();

            return services;
        }
        
    }
}