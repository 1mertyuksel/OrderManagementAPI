using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Entities;
using OrderManagementAPI.Repository.Abstract;

namespace OrderManagementAPI.Repository.Concrete
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AppDbContext _context;

        public OrderDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderDetail orderDetail)
        {
            // Yeni order detail ekle
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            // OrderId'ye göre order details getir
            return await _context.OrderDetails
                                 .Where(od => od.OrderId == orderId)
                                 .ToListAsync();
        }
    }

}
