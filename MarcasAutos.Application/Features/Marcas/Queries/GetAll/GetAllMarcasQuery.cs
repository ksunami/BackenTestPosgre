using MarcasAutos.Application.Common.Models;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MediatR;

namespace MarcasAutos.Application.Features.Marcas.Queries.GetAll
{
    public sealed record GetAllMarcasQuery() : IRequest<Result<List<MarcaAutoDto>>>;
}
