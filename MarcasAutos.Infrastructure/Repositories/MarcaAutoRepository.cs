using MarcasAutos.Application.Abstractions.Persistence;
using MarcasAutos.Domain.Marcas;
using MarcasAutos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MarcasAutos.Infrastructure.Repositories
{
    public sealed class MarcaAutoRepository : IMarcaAutoRepository
    {
        private readonly MarcasDbContext _db;

        public MarcaAutoRepository(MarcasDbContext db) => _db = db;

        public async Task<List<MarcaAuto>> GetAllAsync(CancellationToken ct = default)
            => await _db.Marcas.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync(ct);

        public async Task<MarcaAuto?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _db.Marcas.FirstOrDefaultAsync(x => x.Id == id, ct);
                
    }
}
