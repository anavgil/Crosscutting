using Crosscutting.Persistence.UoW.Abstraction;
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
    }
}