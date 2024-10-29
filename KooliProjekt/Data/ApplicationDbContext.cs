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

        public DbSet<OilType> OilTypes { get; set; }
        public DbSet<SampleBatch> SampleBatches { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<ManufacturingLog> ManufacturingLogs { get; set; }
        public DbSet<BatchComposition> BatchCompositions { get; set; }
        public DbSet<OilBatch> OilBatches { get; set; }
        public DbSet<ProductComponent> ProductComponents { get; set; }

    }
}