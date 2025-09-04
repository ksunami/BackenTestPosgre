using MarcasAutos.Application.DependencyInjection;
using MarcasAutos.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;                       // para Migrate()
using MarcasAutos.Infrastructure.Persistence;             // DbContext
using MarcasAutos.Domain.Marcas;                          // para seed (MarcaAuto.Create)

namespace MarcasAutosAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) DI de capas
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);

            // 2) Controllers + Swagger
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(o =>
            {
                o.SuppressMapClientErrors = false;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 3) CORS (dev)
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("DevAllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            // 4) Kestrel (solo HTTP/1.1) en :5000
            builder.WebHost.ConfigureKestrel(o =>
            {
                o.ListenAnyIP(5000, lo => lo.Protocols = HttpProtocols.Http1);
            });

            var app = builder.Build();

            // 5) Manejo global de errores → ProblemDetails
            app.UseExceptionHandler(errApp =>
            {
                errApp.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    var pd = new ProblemDetails
                    {
                        Title = "Se produjo un error al procesar la solicitud.",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = app.Environment.IsDevelopment() ? ex?.ToString() : "Error interno."
                    };
                    context.Response.StatusCode = pd.Status!.Value;
                    await context.Response.WriteAsJsonAsync(pd);
                });
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("DevAllowAll");
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // REST
            app.MapControllers();

            if(!app.Environment.IsEnvironment("Testing"))
            {
                // 6) Migraciones + seed (solo Development)
                using (var scope = app.Services.CreateScope())
                {
                    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
                    var db = scope.ServiceProvider.GetRequiredService<MarcasDbContext>();

                    if (env.IsDevelopment())
                    {
                        await db.Database.MigrateAsync();

                        if (!await db.Marcas.AnyAsync())
                        {
                            db.Marcas.AddRange(
                                MarcaAuto.Create("Toyota", 1),
                                MarcaAuto.Create("Ford", 1),
                                MarcaAuto.Create("Honda", 1)
                            );
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
            

            await app.RunAsync();
        }
    }
}
