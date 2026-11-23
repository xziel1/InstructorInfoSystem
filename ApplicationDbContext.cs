using InstructorInfoSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InstructorInfoSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(account => account.NormalizedEmail)
                .IsUnique();

            modelBuilder.Entity<UserAccount>()
                .HasIndex(account => account.NormalizedInstructorId)
                .IsUnique();
        }
    }
}
