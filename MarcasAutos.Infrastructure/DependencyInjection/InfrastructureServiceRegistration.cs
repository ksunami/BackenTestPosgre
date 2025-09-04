using MarcasAutos.Application.Abstractions.Persistence;
using MarcasAutos.Infrastructure.Persistence;
using MarcasAutos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarcasAutos.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration config,
            IHostEnvironment env)                
        {
            if (env.IsEnvironment("Testing"))    
            {
                services.AddDbContext<MarcasDbContext>(opt =>
                    opt.UseInMemoryDatabase("MarcasAutos_TestDb"));
            }
            else
            {
                var conn = config.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");
                services.AddDbContext<MarcasDbContext>(opt =>
                    opt.UseNpgsql(conn, b => b.MigrationsAssembly(typeof(MarcasDbContext).Assembly.FullName)));
            }

            services.AddScoped<IMarcaAutoRepository, MarcaAutoRepository>();
            return services;
        }
    }
}
