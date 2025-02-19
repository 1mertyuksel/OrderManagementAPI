using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata;
using OrderManagementAPI.Services.Abstract;

namespace OrderManagementAPI.Services.Concrete
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(ILogger<RabbitMqService> logger)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection(); // Change this line
            _channel = _connection.CreateModel();
            _logger = logger;
        }

        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);

            _logger.LogInformation($"Mesaj kuyruğa gönderildi: {queueName}");
            await Task.CompletedTask;
        }

        public async Task StartListeningAsync()
        {
            _channel.QueueDeclare(queue: "SendMail", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Mesaj alındı: {message}");

                // Burada mesajı işleyebilirsiniz (örneğin, mail gönderme işlemi)
            };

            _channel.BasicConsume(queue: "SendMail", autoAck: true, consumer: consumer);

            _logger.LogInformation("RabbitMQ kuyruğu dinlenmeye başlandı.");
            await Task.CompletedTask;
        }

        public async Task SendMailAsync(string email)
        {
            // Burada RabbitMQ kuyruğuna email gönderme işlemini yapabilirsin.
            Console.WriteLine($"Email gönderme isteği kuyruğa eklendi: {email}");
            await Task.CompletedTask;
        }
    }
}
