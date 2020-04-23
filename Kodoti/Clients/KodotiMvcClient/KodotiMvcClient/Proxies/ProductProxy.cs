using KodotiMvcClient.Common;
using KodotiMvcClient.Proxies.Models.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies
{
    public interface IProductProxy
    {
        Task<DataCollection<ProductDto>> Paged(int page);
        Task Create(ProductCreateDto model, ModelStateDictionary modelState);
        Task<ProductDto> Get(int id);
    }

    public class ProductProxy : IProductProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;

        public ProductProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ProductDto> Get(int id)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.CatalogAPI);
            var response = await client.GetAsync($"products/v1/{id}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ProductDto>();
        }

        public async Task<DataCollection<ProductDto>> Paged(int page)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.CatalogAPI);
            var response = await client.GetAsync($"products/v1?page={page}&pageSize=5");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<DataCollection<ProductDto>>();
        }

        public async Task Create(ProductCreateDto model, ModelStateDictionary modelState)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.CatalogAPI);
            var response = await client.PostAsJsonAsync($"products/v1", model);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var validations = await response.Content.ReadAsAsync<Dictionary<string, List<string>>>();

                foreach (var validation in validations)
                {
                    modelState.AddModelError(validation.Key, validation.Value.First());
                }

                return;
            }

            response.EnsureSuccessStatusCode();
        }
    }
}