using Common.Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Layer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [Route("stores/v{version:apiVersion}")]
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        /// <summary>
        /// Endpoint usado para paginar los Stores
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetStores")]
        public async Task<IActionResult> Paged(int page, int pageSize, string filter)
        {
            var result = await _storeService.Paged(
                page,
                pageSize,
                string.IsNullOrEmpty(filter) ? null : JsonConvert.DeserializeObject<List<QueryFilter>>(filter));
            return Ok(result);
        }
    }
}