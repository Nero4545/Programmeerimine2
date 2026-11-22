using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BeerType> BeerTypes { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<TastingEntry> TastingEntries { get; set; }
        public DbSet<BatchPhoto> BatchPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Kasutaja kustutamisel kanded jäävad alles (kasutaja väli tühjendatakse)
            builder.Entity<LogEntry>()
                .HasOne(e => e.User).WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TastingEntry>()
                .HasOne(e => e.User).WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
