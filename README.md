# Biotekno Case Study

## Overview
This project is an order management API built with .NET 6, which allows clients to list products, create orders, and send email notifications asynchronously via RabbitMQ.

## Technologies Used
- **Framework**: .NET 6
- **Database**: MySQL (Entity Framework Core)
- **Caching**: .NET Memory Cache
- **Message Queue**: RabbitMQ
- **Others**: AutoMapper, Serilog

## Features
- **GetProducts**: Retrieves a list of products (cached for faster access).
- **CreateOrder**: Creates an order, saves it in the database, and triggers an email notification via RabbitMQ.

## Project Workflow
1. Retrieve products via API (cached to reduce database calls).
2. Create an order and send an email request to RabbitMQ.
3. A background service processes email notifications asynchronously.

# Setup & Installation  

1. **Clone the repository:**  
   ```bash  
   git clone https://github.com/1mertyuksel/OrderManagementAPI.git  
   cd OrderManagementAPI  
   ```  

2. **Configure the database connection** in `appsettings.json`.  

3. **Apply migrations:**  
   ```bash  
   dotnet ef database update  
   ```  

4. **Start the application:**  
   ```bash  
   dotnet run  
   ```  

5. **Ensure RabbitMQ is running** for email processing.  

## API Endpoints  

- **GET /api/Product**: Retrieves products (supports category filtering).  
- **POST /api/Order**: Creates an order and sends email notifications.  

## Background Service  

- **MailSenderBackgroundService**: Processes email requests from the RabbitMQ queue.  

## Conclusion  

This project demonstrates a simple order processing API with caching, asynchronous messaging, and background services. You can extend it further as needed.
