using MarcasAutos.Domain.Marcas;

namespace MarcasAutos.Application.Abstractions.Persistence
{
    public interface IMarcaAutoRepository
    {
        Task<List<MarcaAuto>> GetAllAsync(CancellationToken ct = default);
        Task<MarcaAuto?> GetByIdAsync(int id, CancellationToken ct = default);     
    }
}
