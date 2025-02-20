using OrderManagementAPI.Dtos;
using OrderManagementAPI.Entities;
using OrderManagementAPI.Repository.Abstract;
using OrderManagementAPI.Repository;
using OrderManagementAPI.Services.Abstract;
using Org.BouncyCastle.Crypto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace OrderManagementAPI.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,
                            IRabbitMqService rabbitMqService, IMapper mapper)
        {
           
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _rabbitMqService = rabbitMqService;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderRequest createOrderRequest)
        {
            
            var order = new Order
            {
                CustomerName = createOrderRequest.CustomerName,
                CustomerEmail = createOrderRequest.CustomerEmail,
                CustomerGSM = createOrderRequest.CustomerGSM,
                TotalAmount = createOrderRequest.ProductDetails.Sum(p => p.Amount * p.UnitPrice),
                CreateDate = DateTime.UtcNow,
            };

            await _orderRepository.AddAsync(order);
            


            foreach (var detail in createOrderRequest.ProductDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = detail.ProductId,
                    UnitPrice = detail.UnitPrice,
                    Amount = detail.Amount
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }

            var mailMessage = new SendMailRequest
            {
                To = createOrderRequest.CustomerEmail,
                Subject = "Order Confirmation",
                Body = $"Your order with ID {order.Id} has been successfully placed."
            };

            await _rabbitMqService.SendMailAsync(mailMessage);

            return order.Id;
        }
    }

}
