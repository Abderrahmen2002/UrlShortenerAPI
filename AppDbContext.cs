using Microsoft.EntityFrameworkCore;
namespace UrlShortenerAPI
{
    public class AppDbContext : DbContext
    {
        public DbSet<UrlEntry> Urls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=urlshortener;user=root;password=",
                new MySqlServerVersion(new Version(10, 4, 32))
            );
        }
    }
}
