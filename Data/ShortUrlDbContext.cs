using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Models;

namespace UrlShorteningService.Data
{
    public class ShortUrlDbContext : DbContext
    {
        public DbSet<ShortUrlModel> ShortUrls { get; set; }

        public ShortUrlDbContext(DbContextOptions<ShortUrlDbContext> options) : base(options) { }

    }
}
