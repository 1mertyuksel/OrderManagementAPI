using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using OrderManagementAPI.Dtos;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

public class MailSenderBackgroundService : BackgroundService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<MailSenderBackgroundService> _logger;

    public MailSenderBackgroundService(IOptions<SmtpSettings> smtpSettings, ILogger<MailSenderBackgroundService> logger)
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

        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var mailMessage = JsonConvert.DeserializeObject<SendMailRequest>(message);

            _logger.LogInformation("Received mail message for: {Recipient}", mailMessage.To);

            await SendEmailAsync(mailMessage);

            _channel.BasicAck(args.DeliveryTag, multiple: false);
            await Task.CompletedTask;
        };

        _channel.BasicConsume(queue: "SendMailQueue", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task SendEmailAsync(SendMailRequest mailRequest)
    {
        try
        {
            using (var smtpClient = new SmtpClient(_smtpSettings.Server))
            {
                smtpClient.Port = _smtpSettings.Port;
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.FromAddress),
                    Subject = mailRequest.Subject,
                    Body = mailRequest.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(mailRequest.To);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Mail sent successfully to: {Recipient}", mailRequest.To);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email to: {Recipient}", mailRequest.To);
        }
    }
}