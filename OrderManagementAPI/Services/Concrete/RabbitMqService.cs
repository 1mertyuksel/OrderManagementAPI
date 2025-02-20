using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using OrderManagementAPI.Services.Abstract;
using OrderManagementAPI.Dtos;

namespace OrderManagementAPI.Services.Concrete
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(string rabbitMqUri, ILogger<RabbitMqService> logger)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMqUri), 
                DispatchConsumersAsync = true 
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "SendMailQueue",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _logger = logger; 
        }

        public async Task SendMailAsync(SendMailRequest sendMailRequest)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendMailRequest));

            _channel.BasicPublish(exchange: "",
                                  routingKey: "SendMailQueue",
                                  basicProperties: null,
                                  body: body);

            _logger.LogInformation($"Mail kuyruğa eklendi: {sendMailRequest.To}");
            await Task.CompletedTask;
        }
    }
}