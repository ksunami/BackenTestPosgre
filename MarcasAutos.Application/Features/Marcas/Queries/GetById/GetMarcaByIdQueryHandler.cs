using AutoMapper;
using MarcasAutos.Application.Abstractions.Persistence;
using MarcasAutos.Application.Common.Exceptions;
using MarcasAutos.Application.Common.Models;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MediatR;

namespace MarcasAutos.Application.Features.Marcas.Queries.GetById
{
    public sealed class GetMarcaByIdQueryHandler
    : IRequestHandler<GetMarcaByIdQuery, Result<MarcaAutoDto>>
    {
        private readonly IMarcaAutoRepository _repo;
        private readonly IMapper _mapper;

        public GetMarcaByIdQueryHandler(IMarcaAutoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<MarcaAutoDto>> Handle(GetMarcaByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id, ct);
            if (entity is null) throw new NotFoundException("MarcaAuto", request.Id);

            return Result<MarcaAutoDto>.Success(_mapper.Map<MarcaAutoDto>(entity));
        }
    }
}
