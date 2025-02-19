using OrderManagementAPI.Entities;

namespace OrderManagementAPI.Repository.Abstract
{
    public interface IOrderDetailRepository
    {
        Task AddAsync(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetByOrderIdAsync(int orderId);
    }

}
