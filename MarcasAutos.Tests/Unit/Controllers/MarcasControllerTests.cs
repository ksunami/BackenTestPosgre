
using MarcasAutos.API.Controllers;
using MarcasAutos.Application.Common.Exceptions;
using MarcasAutos.Application.Common.Models; // <-- donde está Result<T>
using MarcasAutos.Application.Features.Marcas.Dtos;
using MarcasAutos.Application.Features.Marcas.Queries.GetAll;
using MarcasAutos.Application.Features.Marcas.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace MarcasAutos.Tests.Unit.Controllers;


public class MarcasControllerTests
{
    // ---------- GET /api/marcas ----------
    [Fact]
    public async Task GetAll_Returns_200_with_list()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);

        var expected = new List<MarcaAutoDto>
{
    new(1, "Toyota"),
    new(2, "Ford"),
    new(3, "Honda")
};

        mediator
      .Setup(m => m.Send(It.IsAny<GetAllMarcasQuery>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(Result<List<MarcaAutoDto>>.Success(expected));

        var controller = new MarcasController(mediator.Object);
        var result = await controller.GetAll(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result.Result!;
        ok.Value.Should().BeEquivalentTo(expected);

        mediator.Verify(m => m.Send(It.IsAny<GetAllMarcasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ---------- GET /api/marcas/{id} ----------
    [Fact]
    public async Task GetById_Returns_200_with_item()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);
        var dto = new MarcaAutoDto(10, "Nissan");

        mediator
            .Setup(m => m.Send(It.Is<GetMarcaByIdQuery>(q => q.Id == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<MarcaAutoDto>.Success(dto));

        var controller = new MarcasController(mediator.Object);

        var result = await controller.GetById(10, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result.Result!;
        ok.Value.Should().BeEquivalentTo(dto);

        mediator.Verify(m => m.Send(It.IsAny<GetMarcaByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_When_NotFound_Returns_404()
    {
        var mediator = new Mock<IMediator>(MockBehavior.Strict);

        mediator
            .Setup(m => m.Send(It.IsAny<GetMarcaByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("MarcaAuto", 999));

        var controller = new MarcasController(mediator.Object);

        var result = await controller.GetById(999, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
        mediator.Verify(m => m.Send(It.IsAny<GetMarcaByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

 
}
