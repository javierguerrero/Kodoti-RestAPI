using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KodotiMvcClient.Proxies;
using KodotiMvcClient.Proxies.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KodotiMvcClient.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductProxy _productProxy;
        private readonly IStoreProxy _storeProxy;

        public ProductController(IProductProxy productProxy, IStoreProxy storeProxy)
        {
            _productProxy = productProxy;
            _storeProxy = storeProxy;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _productProxy.Paged(page);
            return View(result);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var result = await _productProxy.Get(id);
            return View(result);
        }

        public async Task<IActionResult> Stores(int id)
        {
            var result = await _storeProxy.GetAllByProductId(id);
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Stores = await _storeProxy.Paged(1);
            return View(new ProductCreateDto { });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto model)
        {
            await _productProxy.Create(model, ModelState);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}