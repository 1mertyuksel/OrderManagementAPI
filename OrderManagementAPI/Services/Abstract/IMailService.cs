using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Abstract
{
    public interface IMailService
    {
        Task SendMailAsync(SendMailRequest request);
    }

}
