using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Entities;
using OrderManagementAPI.Repository.Abstract;

namespace OrderManagementAPI.Repository.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
           
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            
            return await _context.Orders.FindAsync(id);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            
            return await _context.Orders.ToListAsync();
        }
    }

}
