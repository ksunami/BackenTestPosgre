using MarcasAutos.Application.Features.Marcas.Dtos;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarcasAutos.Tests.Integration.Api;

public class MarcasEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public MarcasEndpointsTests(CustomWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task GET_api_marcas_returns_OK_and_seeded_items()
    {
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/api/marcas");
        var body = await resp.Content.ReadAsStringAsync();

        resp.StatusCode.Should().Be(HttpStatusCode.OK, $"body: {body}");

        var data = JsonSerializer.Deserialize<List<MarcaAutoDto>>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        data.Should().NotBeNull();
        data!.Select(x => x.Nombre).Should().Contain(new[] { "Toyota", "Ford", "Honda" });
    }

    // DTO minimal para leer el JSON (ajústalo si tu DTO real tiene otros campos)
    private sealed record MarcaView(int Id, string Nombre);
}
