using KodotiMvcClient.Common;
using KodotiMvcClient.Proxies.Models.Auth;
using System.Net.Http;
using System.Threading.Tasks;

namespace KodotiMvcClient.Proxies
{
    public interface IAuthProxy
    {
        Task<AccessTokenAuthModel> Authenticate(LoginAuthModel model);
    }

    public class AuthProxy : IAuthProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;

        public AuthProxy(ProxyHttpClient proxyHttpClient)
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<AccessTokenAuthModel> Authenticate(LoginAuthModel model)
        {
            var client = _proxyHttpClient.Get(ProxyHttpClient.AuthAPI);
            var response = await client.PostAsJsonAsync("auth/login", model);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<AccessTokenAuthModel>();
        }
    }
}