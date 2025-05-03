using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using Infrastructure.Identity;
using Infrastructure.IntializeDb;
using Infrastructure.Services.LoggerService;
using Infrastructure.Services.MailService;
using Infrastructure.Services.OTPService;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistance;
using Serilog;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLoggerServices(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder loggingBuilder)
        {
            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
            .AddJsonFile("serilog.config.json")
            .Build())
            .Enrich.FromLogContext()
            .CreateLogger();
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);

            services.AddSingleton<ILoggerManager, LoggerManager>();
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenAuthenticationService, TokenAuthenticationService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderConfirmMail, OrderConfirmMail>();
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            // Configure Identity with custom User class
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EcommerceDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        

        

        public static IServiceCollection AddDbInitializer(this IServiceCollection services)
        {

            services.AddScoped<IDbInitializer, DbInitializer>(); // Register IDbInitializer


            return services;
        }
    }
}