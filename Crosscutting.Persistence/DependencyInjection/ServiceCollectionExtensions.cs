using Crosscutting.Persistence.Abstractions.Repositories;
using Crosscutting.Persistence.Abstractions.UoW;
using Crosscutting.Persistence.Repositories.Implementation;
using Crosscutting.Persistence.UoW.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Persistence.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigurePersistenceLayer<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            //https://genericrepository.readthedocs.io/en/latest/getting-started/
            //

            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

            return services;
        }

        public static IServiceCollection ConfigurePersistenceLayer<TContext, TContext1>(this IServiceCollection services)
            where TContext : DbContext
            where TContext1 : DbContext
        {

            services.ConfigurePersistenceLayer<TContext>();
            services.ConfigurePersistenceLayer<TContext1>();

            return services;
        }

        public static IServiceCollection ConfigurePersistenceLayer<TContext, TContext1, TContext2>(this IServiceCollection services)
            where TContext : DbContext
            where TContext1 : DbContext
            where TContext2 : DbContext
        {

            services.ConfigurePersistenceLayer<TContext>();
            services.ConfigurePersistenceLayer<TContext1>();
            services.ConfigurePersistenceLayer<TContext2>();

            return services;
        }
    }
}