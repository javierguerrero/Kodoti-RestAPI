using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KodotiMvcClient.Common
{
    public class ProxyHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public const string AuthAPI = "Auth";
        public const string CatalogAPI = "Catalog";

        public ProxyHttpClient(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient Get(string api)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string>($"APIs:{api}"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (_httpContextAccessor.HttpContext.User != null &&
                _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var access_token = claims.Single(x => x.Type.Equals("access_token")).Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
            }

            return client;
        }
    }
}