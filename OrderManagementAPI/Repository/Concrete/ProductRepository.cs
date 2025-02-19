using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Entities;
using OrderManagementAPI.Repository.Abstract;

namespace OrderManagementAPI.Repository.Concrete
{
    
        public class ProductRepository : IProductRepository
        {
            private readonly AppDbContext _context;

            public ProductRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<ProductDto>> GetByCategoryAsync(string category)
            {
               
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                
                var products = await query.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    Category = p.Category,
                    Unit = p.Unit,
                    UnitPrice = p.UnitPrice
                }).ToListAsync();

                return products;
            }

            public async Task<List<ProductDto>> GetAllAsync()
            {
               
                return await _context.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    Category = p.Category,
                    Unit = p.Unit,
                    UnitPrice = p.UnitPrice
                }).ToListAsync();
            }

            public async Task<Product> GetByIdAsync(int id)
            {
                
                return await _context.Products.FindAsync(id);
            }

            public async Task AddAsync(Product product)
            {
                
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Product product)
            {
                
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
               
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
            }
        }

    
}
