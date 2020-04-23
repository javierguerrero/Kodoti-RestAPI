using KodotiMvcClient.Common;
using KodotiMvcClient.Proxies.Models.Stores;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies
{
    public interface IStoreProxy
    {
        Task<DataCollection<StoreDto>> Paged(int page);
        Task<IEnumerable<StoreDto>> GetAllByProductId(int id);
    }

    public class StoreProxy : IStoreProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;

        public StoreProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<DataCollection<StoreDto>> Paged(int page)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.CatalogAPI);
            var response = await client.GetAsync($"stores/v1?page={page}&pageSize=5");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<DataCollection<StoreDto>>();
        }

        public async Task<IEnumerable<StoreDto>> GetAllByProductId(int id)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.CatalogAPI);
            var response = await client.GetAsync($"products/v1/{id}/stores");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<IEnumerable<StoreDto>>();
        }
    }
}