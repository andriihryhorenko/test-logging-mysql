using Microsoft.EntityFrameworkCore;
using myssql_.netcore_test_app.Model;

namespace myssql_.netcore_test_app.DAL
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");

        }
    }
}
