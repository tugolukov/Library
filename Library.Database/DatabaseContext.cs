using Library.Database.Models;
using Library.Database.Models.RSS;
using Library.Database.Models.Visitor;
using Microsoft.EntityFrameworkCore;

namespace Library.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publishing> Publishings { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        
        public DbSet<RssSource> RssSources { get; set; }
        public DbSet<RssItem> RssItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}