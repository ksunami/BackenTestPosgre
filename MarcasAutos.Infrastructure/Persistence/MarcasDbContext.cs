using MarcasAutos.Domain.Marcas;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Infrastructure.Persistence
{
    public sealed class MarcasDbContext : DbContext
    {
        public MarcasDbContext(DbContextOptions<MarcasDbContext> options) : base(options) { }

        public DbSet<MarcaAuto> Marcas => Set<MarcaAuto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones por assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarcasDbContext).Assembly);

            // Filtro global de soft delete
            modelBuilder.Entity<MarcaAuto>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
