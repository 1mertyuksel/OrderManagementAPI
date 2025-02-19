using OrderManagementAPI.Entities;

namespace OrderManagementAPI.Repository.Abstract
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
    }

}
