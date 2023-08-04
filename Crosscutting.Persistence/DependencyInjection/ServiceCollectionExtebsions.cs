using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Threenine.Data.DependencyInjection;

namespace Crosscutting.Persistence.DependencyInjection
{
    public static class ServiceCollectionExtebsions
    {
        public static IServiceCollection ConfigurePersistenceLayer<TContext>(this IServiceCollection services) where TContext : DbContext
        {

            //https://genericrepository.readthedocs.io/en/latest/getting-started/


            services.AddUnitOfWork<TContext>();
            return services;
        }
    }
}
