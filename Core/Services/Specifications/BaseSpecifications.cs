﻿using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int take { get; set; }
        public bool IsPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>>? expression)
        {
            Criteria = expression;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExpression)
        {
            IncludeExpressions.Add(IncludeExpression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        protected void AddOrderByDescendig(Expression<Func<TEntity, object>> expression)
        {
            OrderByDesc = expression;
        }

        protected void ApplyPagination(int PageIndex, int PageSize)
        {
            IsPagination = true;
            take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }
    }
}