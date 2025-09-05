using System.Collections.Generic;

using Microsoft. EntityFrameworkCore; using System.Collections.Generic;
namespace BLOG.Models
{

    public class BlogContext : DbContext
    {

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<kullanicilar>? Kullanicilar { get; set; }

        public DbSet<kategoriler>? Kategoriler { get; set; }

        public DbSet<paylasimlar>? Paylasimlar { get; set; }

        public DbSet<yorumlar>? Yorumlar { get; set; }
    }
}