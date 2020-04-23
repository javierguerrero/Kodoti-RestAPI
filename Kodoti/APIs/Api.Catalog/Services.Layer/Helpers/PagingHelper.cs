using Common.Layer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Layer.Helpers
{
    public static class PagingHelper
    {
        public static async Task<DataCollection<T>> AsPagedAsync<T>(this IQueryable<T> query, int page, int take)
        {
            var result = new DataCollection<T>();

            if (take == 0)
            {
                take = 20;
            }

            if (take > 20)
            {
                throw new Exception("Exceeded limit");
            }

            result.TotalItems = await query.CountAsync();
            result.PageNumber = page;
            result.PageSize = take;
            result.Items = await query.Skip(take * (page - 1))
                                .Take(take)
                                .ToListAsync();
            result.TotalPages = (int)Math.Ceiling(result.TotalItems / (double)result.PageSize);

            return result;
        }
    }
}