using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Abstract
{
    
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(string category);
    }



}
