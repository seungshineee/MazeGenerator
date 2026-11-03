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
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Results)
                .WithOne()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne<Character>()
                .WithMany()
                .HasForeignKey(u => u.CharacterId);

            modelBuilder.Entity<Character>().HasData(
                new Character { Id = 1, Name = "Жан", ImagePath = "/images/jean.png" },
                new Character { Id = 2, Name = "Нил", ImagePath = "/images/neil.png" },
                new Character { Id = 3, Name = "МоЖань", ImagePath = "/images/moran.png" },
                new Character { Id = 4, Name = "Физаролли", ImagePath = "/images/fizarolli.png" }
            );
        }
    }
}
