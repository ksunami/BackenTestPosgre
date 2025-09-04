using MarcasAutos.Domain.Abstractions;
using MarcasAutos.Domain.Common;
using MarcasAutos.Domain.Events;

namespace MarcasAutos.Domain.Marcas
{
    public sealed class MarcaAuto : AggregateRoot<int>, IAuditable, ISoftDelete, IHasConcurrencyToken
    {
        private const int NombreMaxLength = 120;

        public string Nombre { get; private set; } = default!;

        // Auditoría
        public DateTime DateCreated { get; private set; }
        public int CreatedBy { get; private set; }
        public DateTime? DateModified { get; private set; }
        public int? ModifiedBy { get; private set; }

        // Soft delete y concurrencia
        public bool IsDeleted { get; private set; }
        public byte[]? RowVersion { get; private set; } = Array.Empty<byte>();

        // ctor privado para ORM/serialización
        private MarcaAuto() { }

        private MarcaAuto(string nombre, int createdBy)
        {
            SetNombre(nombre);
            DateCreated = DateTime.UtcNow;
            CreatedBy = createdBy;
            Raise(new MarcaAutoCreated(nombre));
        }

        public static MarcaAuto Create(string nombre, int createdBy) => new(nombre, createdBy);

        public void Rename(string nuevoNombre, int modifiedBy)
        {
            SetNombre(nuevoNombre);
            Touch(modifiedBy);
            Raise(new MarcaAutoUpdated(Id, Nombre));
        }

        public void SoftDelete(int modifiedBy)
        {
            if (IsDeleted) return;
            IsDeleted = true;
            Touch(modifiedBy);
            Raise(new MarcaAutoDeleted(Id));
        }

        public void Restore(int modifiedBy)
        {
            if (!IsDeleted) return;
            IsDeleted = false;
            Touch(modifiedBy);
            Raise(new MarcaAutoUpdated(Id, Nombre));
        }

        public void SetRowVersion(byte[]? rowVersion) => RowVersion = rowVersion ?? Array.Empty<byte>();

        private void SetNombre(string nombre)
            => Nombre = ValidateNombre(nombre);

        private static string ValidateNombre(string nombre)
        {
            Guard.AgainstNullOrWhiteSpace(nombre, nameof(nombre), NombreMaxLength);
            return nombre.Trim();
        }

        private void Touch(int modifiedBy)
        {
            DateModified = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }
    }
}
