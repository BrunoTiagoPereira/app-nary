using AppNary.Core.Communication.Pipelines;
using AppNary.Core.Transaction;
using AppNary.Data;
using AppNary.Data.Repositories;
using AppNary.Domain.Recipes.Repositories;
using AppNary.Domain.Users.Commands.Requests;
using AppNary.Domain.Users.Managers;
using AppNary.Domain.Users.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsPricing.Domain.Users.Managers;

namespace AppNary.CrossCutting.IoC
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationPipelineBehavior<,>))
                ;

            return services;
        }

        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<DbContext, DatabaseContext>(builder =>
            {
                builder.UseSqlServer(configuration.GetConnectionString("SqlServer"), op => op.EnableRetryOnFailure(3));
            }, ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddMediatR((c) =>
            {
                c.Lifetime = ServiceLifetime.Scoped;
                c.RegisterServicesFromAssembly(typeof(CreateUserCommandRequest).Assembly);
            });

            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IRecipeRepository, RecipeRepository>()
                ;

            services
                .AddScoped<IUserAccessorManager, UserAccessorManager>()
                ;

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateUserCommandRequest).Assembly);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCoreServices()
                .AddDomainServices()
                .AddDataServices(configuration)
                .AddValidators()
                ;

            return services;
        }
    }
}