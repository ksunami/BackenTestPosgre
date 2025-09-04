using AutoMapper;
using MarcasAutos.Application.Abstractions.Persistence;
using MarcasAutos.Application.Common.Models;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MediatR;

namespace MarcasAutos.Application.Features.Marcas.Queries.GetAll
{
    public sealed class GetAllMarcasQueryHandler
    : IRequestHandler<GetAllMarcasQuery, Result<List<MarcaAutoDto>>>
    {
        private readonly IMarcaAutoRepository _repo;
        private readonly IMapper _mapper;

        public GetAllMarcasQueryHandler(IMarcaAutoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<List<MarcaAutoDto>>> Handle(GetAllMarcasQuery request, CancellationToken ct)
        {
            var entities = await _repo.GetAllAsync(ct);
            var dtos = _mapper.Map<List<MarcaAutoDto>>(entities);
            return Result<List<MarcaAutoDto>>.Success(dtos);
        }
    }
}
