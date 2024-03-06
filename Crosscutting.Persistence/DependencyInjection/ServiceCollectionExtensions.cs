using Crosscutting.Persistence.Abstractions.UoW;
using Crosscutting.Persistence.Repositories;
using Crosscutting.Persistence.UoW;
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

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            return services;
        }
    }
}
