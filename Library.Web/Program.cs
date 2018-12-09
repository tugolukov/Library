using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Database;
using Library.Web.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Library.Web
{
    public class Program
    {
        /// <summary/>
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var host = BuildWebHost(args)
                .MigrateDatabase<DatabaseContext>();
            host.Run();
        }

        /// <summary>
        /// Построение хоста
        /// </summary>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}