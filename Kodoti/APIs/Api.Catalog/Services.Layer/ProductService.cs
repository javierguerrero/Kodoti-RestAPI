using AutoMapper;
using Common.Layer;
using Domain.Dto.Layer;
using Domain.Layer;
using Microsoft.EntityFrameworkCore;
using Persistence.Layer;
using Services.Layer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Layer
{
    public interface IProductService
    {
        Task<bool> UniqueName(string name);

        Task<ProductDto> Get(int id);

        Task<DataCollection<ProductDto>> Paged(int page, int take, IEnumerable<QueryFilter> filters = null);

        Task<ResponseHelper<int>> Create(ProductCreateDto model);

        Task<ResponseHelper> Update(int id, ProductUpdateDto model);

        Task<ResponseHelper> Partial(int id, ProductPartialDto model);

        Task<ResponseHelper> Delete(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UniqueName(string name)
        {
            return await _context.Products.AnyAsync(x => x.Name.Equals(name));
        }

        public async Task<ProductDto> Get(int id)
        {
            var result = new ProductDto();
            try
            {
                var query = await _context.Products
                                          .Include(x => x.ProductStores)
                                            .ThenInclude(x => x.Store)
                                          .SingleAsync(x => x.ProductId == id);
                result = Mapper.Map<ProductDto>(query);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public async Task<DataCollection<ProductDto>> Paged(int page, int take, IEnumerable<QueryFilter> filters = null)
        {
            var result = new DataCollection<ProductDto>();
            try
            {
                var query = _context.Products.OrderBy(x => x.ProductId).AsQueryable();

                if (filters != null)
                {
                    foreach (var filter in filters)
                    {
                        if (filter.Field.Equals("isEnable"))
                        {
                            if (filter.Type == QueryFilterType.eq)
                            {
                                query = query.Where(x => x.IsEnabled == Convert.ToBoolean(filter.Value));
                            }

                            if (filter.Type == QueryFilterType.NotEq)
                            {
                                query = query.Where(x => x.IsEnabled == Convert.ToBoolean(filter.Value));
                            }
                        }
                    }
                }

                var data = await query.AsPagedAsync(page, take);

                result = Mapper.Map<DataCollection<ProductDto>>(data);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public async Task<ResponseHelper<int>> Create(ProductCreateDto model)
        {
            var result = new ResponseHelper<int>();
            try
            {
                var entry = Mapper.Map<Product>(model);

                entry.CreatedAt = DateTime.UtcNow;
                entry.UpdatedAt = DateTime.UtcNow;

                await _context.AddAsync(entry);
                await _context.SaveChangesAsync();

                if (model.Stores != null && model.Stores.Any())
                {
                    await _context.AddRangeAsync(model.Stores.Select(x => new ProductStore
                    {
                        StoreId = x,
                        ProductId = entry.ProductId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    })); ;
                }

                await _context.SaveChangesAsync();

                result.Result = entry.ProductId;
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public async Task<ResponseHelper> Update(int id, ProductUpdateDto model)
        {
            var result = new ResponseHelper();
            try
            {
                var originalEntry = await _context.Products.SingleAsync(x => x.ProductId == id);
                Mapper.Map(model, originalEntry);

                originalEntry.UpdatedAt = DateTime.UtcNow;

                _context.Update(originalEntry);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public async Task<ResponseHelper> Partial(int id, ProductPartialDto model)
        {
            var result = new ResponseHelper();
            try
            {
                var originalEntry = await _context.Products.SingleAsync(x => x.ProductId == id);
                originalEntry.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(model.Name))
                {
                    originalEntry.Name = model.Name;
                }

                if (!string.IsNullOrEmpty(model.Description))
                {
                    originalEntry.Description = model.Description;
                }

                if (model.IsEnabled.HasValue)
                {
                    originalEntry.IsEnabled = model.IsEnabled.Value;
                }

                _context.Update(originalEntry);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public async Task<ResponseHelper> Delete(int id)
        {
            var result = new ResponseHelper();
            try
            {
                _context.Products.Remove(new Product { ProductId = id });
                await _context.SaveChangesAsync();
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }
    }
}