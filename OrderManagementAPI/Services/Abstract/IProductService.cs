using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Abstract
{
    // IProductService Interface
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(string category);
    }



}
