namespace OrderManagementAPI.Services.Abstract
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync<T>(string queueName, T message);
        Task StartListeningAsync();
        Task SendMailAsync(string email);
    }

}
