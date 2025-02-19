using Newtonsoft.Json;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Services.Abstract;
using RabbitMQ.Client;
using System.Text;

namespace OrderManagementAPI.Services.Concrete
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly IConnection _rabbitConnection;

        public MailService(ILogger<MailService> logger, IConnection rabbitConnection)
        {
            _logger = logger;
            _rabbitConnection = rabbitConnection;
        }

        public async Task SendMailAsync(SendMailRequest request)
        {
            // RabbitMQ kuyruğuna mail gönderme işlemi
            using (var channel = _rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(queue: "SendMail", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var message = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "SendMail",
                                     basicProperties: null,
                                     body: body);

                _logger.LogInformation($"Mail request sent to RabbitMQ for {request.To}");
            }
        }
    }

}
