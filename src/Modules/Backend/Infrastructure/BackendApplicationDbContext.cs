using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure
{
    public sealed class BackendApplicationDbContext : DbContext
    {
        public BackendApplicationDbContext(
            DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null) 
            { 
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BackendApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
