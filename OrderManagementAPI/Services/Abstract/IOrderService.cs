using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Abstract
{
    
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderRequest createOrderRequest);
    }


}

