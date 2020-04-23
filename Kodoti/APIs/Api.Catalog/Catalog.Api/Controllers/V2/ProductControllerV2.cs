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
    [ApiVersion("2")]
    [ApiController]
    [Route("products/v{version:apiVersion}")]
    public class ProductControllerV2 : Controller
    {
        private readonly IProductService _productService;

        public ProductControllerV2(IProductService productService)
        {
            _productService = productService;
        }

        // products/1
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _productService.Get(id);

            if (result.ProductId == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}