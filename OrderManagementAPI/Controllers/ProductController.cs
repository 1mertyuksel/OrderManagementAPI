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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetProducts([FromQuery] string? category = null)
        {
            var products = await _productService.GetProductsAsync(category);

            if (products == null || products.Count == 0)
            {
                return NotFound(new ApiResponse<List<ProductDto>>
                {
                    Status = Status.Failed,
                    ResultMessage = "Ürün bulunamadı.",
                    ErrorCode = 404,
                    Data = null
                });
            }

            return Ok(new ApiResponse<List<ProductDto>>
            {
                Status = Status.Success,
                ResultMessage = "Ürünler başarıyla getirildi.",
                Data = products
            });
        }
    }
}
