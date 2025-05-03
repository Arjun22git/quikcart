using Application.Ecom.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Collections.Concurrent;
using MailKit.Net.Smtp;
using System.Security.Cryptography;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.OTPService
{
    public class OtpService : IOtpService
    {
        private readonly IEcommerceDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _otpValidity = TimeSpan.FromMinutes(5);
        private readonly UserManager<User> _userManager;
        private readonly ILogger<OtpService> _logger;

        public OtpService(IEcommerceDbContext dbContext, IConfiguration configuration, UserManager<User> userManager, ILogger<OtpService> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> GenerateOtpAsync(string email)
        {
            var otp = GenerateRandomOtp();
            var createdAt = DateTime.UtcNow;

            var otpEntry = new Otp
            {
                Email = email,
                OtpCode = otp,
                CreatedAt = createdAt
            };

            _dbContext.Otps.Add(otpEntry);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Generated OTP for {email}: {otp}");

            return otp;
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var otpEntry = await _dbContext.Otps
                .Where(o => o.Email == email && o.OtpCode == otp)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpEntry != null && DateTime.UtcNow - otpEntry.CreatedAt <= _otpValidity)
            {
                _dbContext.Otps.Remove(otpEntry); 
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task SendOtpEmailAsync(string email, string otp)
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
                message.Subject = "Your OTP Code valid for 5 minutes is:";
                message.Body = new TextPart("plain")
                {
                    Text = $"Your OTP code is: {otp}"
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

        private string GenerateRandomOtp()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[4];
                rng.GetBytes(buffer);
                var number = BitConverter.ToUInt32(buffer, 0);
                var otp = (number % 1000000).ToString("D6"); // Generate a 6-digit OTP
                return otp;
            }
        }
    }

}
