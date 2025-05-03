using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
namespace Infrastructure.Services.MailService
{
    public class OrderConfirmMail : IOrderConfirmMail
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILoggerManager _logger;
        public OrderConfirmMail(IEcommerceDbContext dbContext,IConfiguration configuration, ILoggerManager loggerManager)
        {
            _logger = loggerManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }
       

        public async Task SendConfirmEmailAsync(string email, Order order)
        {
            try
            {
                var smtpHost = _configuration["Smtp:Host"];
                var smtpPortString = _configuration["Smtp:Port"];
                var smtpUser = _configuration["Smtp:Username"];
                var smtpPass = _configuration["Smtp:Password"];
                var fromAddress = _configuration["Smtp:From"];

                if (string.IsNullOrEmpty(smtpHost) ||
                    string.IsNullOrEmpty(smtpPortString) ||
                    string.IsNullOrEmpty(smtpUser) ||
                    string.IsNullOrEmpty(smtpPass) ||
                    string.IsNullOrEmpty(fromAddress))
                {
                    throw new InvalidOperationException("SMTP configuration is missing or incomplete.");
                }

                if (!int.TryParse(smtpPortString, out int smtpPort))
                {
                    throw new InvalidOperationException("SMTP Port is not a valid integer.");
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("MyAddress", fromAddress));
                message.To.Add(new MailboxAddress("User", email));
                message.Subject = "Order Confirmed!";
                message.Body = new TextPart("plain")
                {
                    Text =  $"Dear Customer,\n\n" +
                            $"Thank you for ordering from us!\n\n" +
                            $"Your order has been confirmed.\n" +
                            $"Order ID: {order.OrderId}\n" +
                            $"Expected Delivery Date: {order.DeliveryDate}\n\n" +
                            $"If you have any questions, please do not hesitate to contact us at queries@ecommerce.com .\n\n"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(smtpUser, smtpPass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Use ILoggerManager to log exceptions
                _logger.LogError($"Error sending OTP email: {ex.Message}");
                throw;
            }
        }
    }
}
