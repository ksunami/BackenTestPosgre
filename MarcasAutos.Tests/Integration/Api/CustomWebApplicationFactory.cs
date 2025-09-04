using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MarcasAutos.Infrastructure.Persistence;
using MarcasAutos.Domain.Marcas;

namespace MarcasAutos.Tests.Integration.Api
{
    public class CustomWebApplicationFactory : WebApplicationFactory<MarcasAutosAPI.Program>
    {
        // Comparte el mismo almacén InMemory entre proveedores de servicios
        private static readonly InMemoryDatabaseRoot _dbRoot = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // 1) Quita cualquier registro previo del DbContext (por si Infra ya registró algo)
                var dbOptions = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MarcasDbContext>));
                if (dbOptions is not null) services.Remove(dbOptions);

                var ctxDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(MarcasDbContext));
                if (ctxDescriptor is not null) services.Remove(ctxDescriptor);

                // 2) Registra InMemory usando el root compartido
                services.AddDbContext<MarcasDbContext>(opt =>
                    opt.UseInMemoryDatabase("MarcasAutos_TestDb", _dbRoot));

                // 3) Construye proveedor (temporal) y siembra usando el root compartido
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<MarcasDbContext>();

                // Limpia y crea
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                if (!db.Marcas.Any())
                {
                    db.Marcas.AddRange(
                        MarcaAuto.Create("Toyota", 1),
                        MarcaAuto.Create("Ford", 1),
                        MarcaAuto.Create("Honda", 1)
                    );
                    db.SaveChanges();
                }
            });
        }
    }
}
