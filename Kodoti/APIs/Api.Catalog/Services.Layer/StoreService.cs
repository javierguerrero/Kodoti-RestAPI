using AutoMapper;
using Common.Layer;
using Domain.Dto.Layer;
using Microsoft.EntityFrameworkCore;
using Persistence.Layer;
using Services.Layer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Layer
{
    public interface IStoreService
    {
        Task<DataCollection<StoreDto>> Paged(int page, int take, IEnumerable<QueryFilter> filters = null);
        Task<IEnumerable<StoreDto>> GetAllByProductId(int productId);
    }

    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataCollection<StoreDto>> Paged(int page, int take, IEnumerable<QueryFilter> filters = null)
        {
            var result = new DataCollection<StoreDto>();
            try
            {
                var query = _context.Stores.OrderBy(x => x.StoreId).AsQueryable();
                var data = await query.AsPagedAsync(page, take);
                result = Mapper.Map<DataCollection<StoreDto>>(data);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public async Task<IEnumerable<StoreDto>> GetAllByProductId(int productId)
        {
            var result = new List<StoreDto>();
            try
            {
                var data = await _context.Stores.Where(x => x.ProductStores.Any(y => y.ProductId == productId)).ToListAsync();
                result = Mapper.Map<List<StoreDto>>(data);
            }
            catch (Exception e)
            {
            }
            return result;
        }
    }
}