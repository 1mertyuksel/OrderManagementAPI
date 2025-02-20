using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Repository;
using OrderManagementAPI.Repository.Abstract;
using OrderManagementAPI.Repository.Concrete;
using OrderManagementAPI.Services.Abstract;
using OrderManagementAPI.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString)
);

var rabbitMqUri = "amqps://yftlcytc:Nu3dls-7titjzOljhYMcukdtDsa6lnuR@moose.rmq.cloudamqp.com/yftlcytc";

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddSingleton<IRabbitMqService>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMqService>>();
    return new RabbitMqService(rabbitMqUri, logger);
});

builder.Services.AddHostedService<MailSenderBackgroundService>();
// SMTP ayarlarýný yapýlandýr
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Diðer servisler
builder.Services.AddHostedService<MailSenderBackgroundService>();

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();