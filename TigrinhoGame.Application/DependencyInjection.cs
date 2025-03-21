using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TigrinhoGame.Application.DTOs;
using TigrinhoGame.Application.Interfaces;
using TigrinhoGame.Application.Services;
using TigrinhoGame.Application.Validators;

namespace TigrinhoGame.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register Services
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<JwtService>();
            services.AddSingleton<TransactionMonitoringService>();

            // Register Validators
            services.AddScoped<IValidator<CreatePlayerDto>, CreatePlayerDtoValidator>();
            services.AddScoped<IValidator<PlayerLoginDto>, PlayerLoginDtoValidator>();
            services.AddScoped<IValidator<SpinRequestDto>, SpinRequestDtoValidator>();

            // Register FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
} 