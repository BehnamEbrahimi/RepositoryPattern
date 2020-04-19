using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Domain;
using Core.Interfaces;
using Core.Types;

namespace Persistence.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Vehicle> ApplyVehicleFiltering(this IQueryable<Vehicle> query, VehicleFilter filter)
        {
            if (filter.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == filter.MakeId.Value);

            if (filter.ModelId.HasValue)
                query = query.Where(v => v.ModelId == filter.ModelId.Value);

            if (!string.IsNullOrWhiteSpace(filter.UserId))
                query = query.Where(v => v.UserId == filter.UserId);

            return query;
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IFilter filter, Dictionary<string, Expression<Func<T, object>>> path)
        {
            if (string.IsNullOrWhiteSpace(filter.SortBy) || !path.ContainsKey(filter.SortBy))
                return query;

            if (filter.IsSortDescending)
                return query.OrderByDescending(path[filter.SortBy]);
            else
                return query.OrderBy(path[filter.SortBy]);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IFilter filter)
        {
            if (filter.Page <= 0)
                filter.Page = 1;

            if (filter.PageSize <= 0)
                filter.PageSize = 5;

            return query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
        }
    }
}