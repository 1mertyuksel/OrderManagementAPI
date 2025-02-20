using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Abstract
{
    public interface IRabbitMqService
    {
       
        Task SendMailAsync(SendMailRequest sendMailRequest);
    }

}
