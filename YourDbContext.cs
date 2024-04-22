using Microsoft.EntityFrameworkCore;
using RestApi_s24412.Models;

namespace RestApi_s24412
{
    public class YourDbContext : DbContext
    {
        public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

        public DbSet<Animal> Animals { get; set; }
    }


}
