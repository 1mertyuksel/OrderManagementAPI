using OrderManagementAPI.Entities;

namespace OrderManagementAPI.Dtos
{
    public class CreateOrderRequest
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGSM { get; set; }
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
