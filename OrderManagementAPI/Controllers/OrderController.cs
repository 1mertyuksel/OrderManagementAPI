using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Services.Abstract;
using System.Threading.Tasks;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create-order")]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orderId = await _orderService.CreateOrderAsync(request);

            if (orderId <= 0)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Status = Status.Failed,
                    ResultMessage = "Sipariş oluşturulamadı.",
                    ErrorCode = 400,
                    Data = 0
                });
            }

            return Ok(new ApiResponse<int>
            {
                Status = Status.Success,
                ResultMessage = "Sipariş başarıyla oluşturuldu.",
                Data = orderId
            });
        }
    }
}
