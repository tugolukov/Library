using Library.Parser.Interfaces;
using Library.Parser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Parser
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddParser(this IServiceCollection services)
        {
            services.AddScoped<IOzonParser, OzonParser>();
            services.AddScoped<IParser, Services.Parser>();
            services.AddScoped<IEksmoParser, EksmoParser>();
            services.AddScoped<IPiterParser, PiterParser>();
            return services;
        }
    }
} 