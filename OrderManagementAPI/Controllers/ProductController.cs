using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Services.Abstract;
using OrderManagementAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Belirtilen kategoriye göre ürünleri getirir.
        /// </summary>
        /// <param name="category">Opsiyonel kategori adı</param>
        /// <returns>Ürünlerin listesi</returns>
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] string? category = null)
        {
            var products = await _productService.GetProductsAsync(category);

            if (products == null || products.Count == 0)
            {
                return NotFound(new { message = "Ürün bulunamadı." });
            }

            return Ok(products);
        }
    }
}
