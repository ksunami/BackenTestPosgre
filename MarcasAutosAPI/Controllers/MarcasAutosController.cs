using MediatR;
using Microsoft.AspNetCore.Mvc;
using MarcasAutos.Application.Features.Marcas.Dtos;
using MarcasAutos.Application.Features.Marcas.Queries.GetAll;
using MarcasAutos.Application.Features.Marcas.Queries.GetById;
using MarcasAutos.Application.Common.Exceptions;

namespace MarcasAutos.API.Controllers
{
    [ApiController]
    [Route("api/marcas")]
    public class MarcasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MarcasController(IMediator mediator) => _mediator = mediator;

        /// <summary>Retorna la lista completa de marcas.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MarcaAutoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MarcaAutoDto>>> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllMarcasQuery(), ct);
            return Ok(result.Value);
        }

        /// <summary>Retorna una marca por Id.</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MarcaAutoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MarcaAutoDto>> GetById(int id, CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetMarcaByIdQuery(id), ct);
                return Ok(result.Value);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
        
    }
}
