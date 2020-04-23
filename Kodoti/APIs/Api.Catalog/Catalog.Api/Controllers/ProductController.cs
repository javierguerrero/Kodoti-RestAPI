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
    [ApiVersion("1", Deprecated = true)]
    [ApiController]
    [Produces("application/json")]
    [Route("products/v{version:apiVersion}")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
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


        /// <summary>
        /// Endpoint usado para paginar los productos
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Paged(int page, int pageSize, string filter)
        {
            var result = await _productService.Paged(
                page, 
                pageSize, 
                string.IsNullOrEmpty(filter) ? null : JsonConvert.DeserializeObject<List<QueryFilter>>(filter));
            return Ok(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="201">Cuando el recurso es creado correctamente. Adicionalmente, retorna el recurso creado (objeto).</response>
        /// <response code="400">Cuando los parámetros de entrada no ha podido ser validados.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.Create(model);
            if (!result.IsSuccess)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }

            var lastEntry = await _productService.Get(result.Result);

            return CreatedAtRoute("GetProductById", new { id = result.Result }, lastEntry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]ProductUpdateDto model)
        {
            var result = await _productService.Update(id, model);
            if (!result.IsSuccess)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Partial(int id, [FromBody]ProductPartialDto model)
        {
            var result = await _productService.Partial(id, model);
            if (!result.IsSuccess)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (!result.IsSuccess)
            {
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError));
            }

            return NoContent();
        }
    }
}