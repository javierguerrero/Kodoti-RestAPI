using Common.Layer;
using Domain.Dto.Layer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Layer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [ApiController]
    [Produces("application/json")]
    [Route("products/v{version:apiVersion}/{id}/stores")]
    public class ProductStoreController : Controller
    {
        private readonly IStoreService _storeService;

        public ProductStoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // products/1
        [HttpGet(Name = "GetProductStoreById")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _storeService.GetAllByProductId(id);
            return Ok(result);
        }
    }
}