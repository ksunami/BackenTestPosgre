using MarcasAutos.Domain.Marcas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarcasAutos.Infrastructure.Persistence.Configurations
{
    public sealed class MarcaAutoConfiguration : IEntityTypeConfiguration<MarcaAuto>
    {
        public void Configure(EntityTypeBuilder<MarcaAuto> b)
        {
            b.ToTable("MarcasAutos");

            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(120);

            // Concurrencia optimista
            b.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            // Auditoría básica (valores por defecto si quieres)
            b.Property(x => x.DateCreated).IsRequired();
            b.Property(x => x.CreatedBy).IsRequired();

            // Seed estable (fechas fijas para evitar nuevas migraciones)
            b.HasData(
                new MarcaAutoSeed(1, "Toyota"),
                new MarcaAutoSeed(2, "Ford"),
                new MarcaAutoSeed(3, "Chevrolet"),
                new MarcaAutoSeed(4, "Honda"),
                new MarcaAutoSeed(5, "Nissan")
            );
        }

        // Clase privada auxiliar para seed sin exponer lógica de dominio
        private sealed class MarcaAutoSeed
        {
            public MarcaAutoSeed(int id, string nombre)
            {
                Id = id;
                Nombre = nombre;
                DateCreated = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                CreatedBy = 1;
                IsDeleted = false;
                RowVersion = Array.Empty<byte>();
            }
            public int Id { get; }
            public string Nombre { get; }
            public DateTime DateCreated { get; }
            public int CreatedBy { get; }
            public DateTime? DateModified { get; } = null;
            public int? ModifiedBy { get; } = null;
            public bool IsDeleted { get; }
            public byte[]? RowVersion { get; }
        }
    }
}
