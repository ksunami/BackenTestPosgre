using MarcasAutos.Application.Common.Models;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MediatR;

namespace MarcasAutos.Application.Features.Marcas.Queries.GetById
{
    public sealed record GetMarcaByIdQuery(int Id) : IRequest<Result<MarcaAutoDto>>;
}
