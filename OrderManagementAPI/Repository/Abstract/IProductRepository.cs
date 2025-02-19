using OrderManagementAPI.Dtos;
using OrderManagementAPI.Entities;

namespace OrderManagementAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetByCategoryAsync(string category);
        Task<List<ProductDto>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }

}
