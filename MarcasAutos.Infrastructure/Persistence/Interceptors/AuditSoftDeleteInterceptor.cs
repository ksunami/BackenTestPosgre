using MarcasAutos.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MarcasAutos.Infrastructure.Persistence.Interceptors
{
    public sealed class AuditSoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly Func<int> _currentUserId; // inyecta proveedor de usuario actual

        public AuditSoftDeleteInterceptor(Func<int> currentUserId)
            => _currentUserId = currentUserId;

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var ctx = eventData.Context;
            if (ctx is null) return base.SavingChanges(eventData, result);

            foreach (var entry in ctx.ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable aud)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("DateCreated").CurrentValue ??= DateTime.UtcNow;
                        entry.Property("CreatedBy").CurrentValue ??= _currentUserId();
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property("DateModified").CurrentValue = DateTime.UtcNow;
                        entry.Property("ModifiedBy").CurrentValue = _currentUserId();
                    }
                }
            }

            return base.SavingChanges(eventData, result);
        }
    }
}
