using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SimpleSystemsManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;
using TigrinhoGame.Infrastructure.Repositories;
using TigrinhoGame.Infrastructure.Services;
using TigrinhoGame.Infrastructure.Services.AWS;

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

            // Configuração do AWS SDK
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSecretsManager>();
            services.AddAWSService<IAmazonSimpleSystemsManagement>();

            // Registro dos serviços AWS
            services.AddScoped<IAwsStorageService, AwsStorageService>();
            services.AddScoped<IGameAssetsService, GameAssetsService>();
            services.AddScoped<ISecretsService, SecretsService>();

            return services;
        }
    }
} 