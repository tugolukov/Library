using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Web.Utils
{
    /// <summary>
    /// Методы расширения для IWebHost
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Миграция БД
        /// </summary>
        /// <typeparam name="TContext">Тип контекста БД</typeparam>
        /// <param name="host"><see cref="IWebHost"/></param>
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
                dbContext.Database.Migrate();
            }

            return host;
        }
    }
}