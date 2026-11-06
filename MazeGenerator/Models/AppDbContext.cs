using Microsoft.EntityFrameworkCore;

namespace MazeGenerator.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<LevelResults> Results => Set<LevelResults>();
        public DbSet<Character> Character => Set<Character>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
