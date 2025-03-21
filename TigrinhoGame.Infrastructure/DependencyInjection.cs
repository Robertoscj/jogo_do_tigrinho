using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;
using TigrinhoGame.Infrastructure.Repositories;
using TigrinhoGame.Infrastructure.Services;

namespace TigrinhoGame.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure database
            services.Configure<DatabaseConfig>(options =>
            {
                options.ConnectionString = configuration.GetSection("DatabaseConfig:ConnectionString").Value
                    ?? throw new InvalidOperationException("Connection string not found in configuration.");
            });
            services.AddSingleton<DapperContext>();

            // Register repositories
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ISpinRepository, SpinRepository>();
            services.AddScoped<ISymbolRepository, SymbolRepository>();
            services.AddScoped<IPayLineRepository, PayLineRepository>();

            // Register services
            services.AddScoped<IRNGService, RNGService>();

            return services;
        }
    }
} 