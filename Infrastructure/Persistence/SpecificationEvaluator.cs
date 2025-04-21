using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, Tkey>
            (IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity, Tkey> spec)
            where TEntity : BaseEntity<Tkey>
        {

            var Query = inputQuery;

            if (spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(spec.OrderByDesc);


            if (spec.IsPagination)
            {
                Query = Query.Skip(spec.Skip).Take(spec.take);
            }


            Query = spec.IncludeExpressions.Aggregate(Query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return Query;
        }
    }
}