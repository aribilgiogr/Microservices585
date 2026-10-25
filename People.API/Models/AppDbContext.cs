using Microsoft.EntityFrameworkCore;

namespace People.API.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Person> People { get; set; }
    }
}
