using Crosscutting.Persistence.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Crosscutting.Persistence.Specifications;

public class SpecificationBuilder<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;


        // modify the IQueryable using the specification's criteria expression
        if (spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria);
        }

        // Includes all expression-based includes
        query = spec.Includes.Aggregate(query,
                                (current, include) => current.Include(include));

        // Include any string-based include statements
        query = spec.IncludeStrings.Aggregate(query,
                                (current, include) => current.Include(include));

        // Apply ordering if expressions are set
        if (spec.OrderBy is not null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.GroupBy is not null)
        {
            query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
        }

        // Apply paging if enabled
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip)
                         .Take(spec.Take);
        }

        return query;
    }
}
