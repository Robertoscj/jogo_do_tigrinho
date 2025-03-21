using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;
using TigrinhoGame.Application.Services;
using TigrinhoGame.Application.Validators;
using TigrinhoGame.Domain.Interfaces;
using TigrinhoGame.Infrastructure.Data;
using TigrinhoGame.Infrastructure.Repositories;

namespace TigrinhoGame.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<JwtService>();
            services.AddScoped<TransactionMonitoringService>();

            // Repositories
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<ISpinRepository, SpinRepository>();

            // Validators
            services.AddScoped<IValidator<CreatePlayerDto>, CreatePlayerDtoValidator>();
            services.AddScoped<IValidator<PlayerLoginDto>, PlayerLoginDtoValidator>();
            services.AddScoped<IValidator<SpinRequestDto>, SpinRequestDtoValidator>();

            return services;
        }
    }
} 