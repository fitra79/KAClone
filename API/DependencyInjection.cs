using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using Application.Common.Interfaces;
using Infrastructure.Persistence;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services
            .AddValidatorsFromAssemblyContaining<IApplicationDbContext>()
            .AddFluentValidationClientsideAdapters();

            return services;

        }
    }
}