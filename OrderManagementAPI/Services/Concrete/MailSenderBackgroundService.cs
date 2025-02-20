using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using OrderManagementAPI.Dtos;

public class MailSenderBackgroundService : BackgroundService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public MailSenderBackgroundService()
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri("amqps://yftlcytc:Nu3dls-7titjzOljhYMcukdtDsa6lnuR@moose.rmq.cloudamqp.com/yftlcytc"), 
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "SendMailQueue",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var mailMessage = JsonConvert.DeserializeObject<SendMailRequest>(message);

            // Gerçek bir mail servisiyle burada mail gönderme işlemi yapılır
            Console.WriteLine($"Mail Gönderildi: {mailMessage.To}");

            _channel.BasicAck(args.DeliveryTag, multiple: false);
            await Task.CompletedTask;
        };

        _channel.BasicConsume(queue: "SendMailQueue", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }
}
