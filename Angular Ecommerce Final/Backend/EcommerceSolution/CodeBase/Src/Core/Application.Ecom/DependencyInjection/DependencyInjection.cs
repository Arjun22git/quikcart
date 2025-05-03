using Application.Ecom.Authentication.Commands.GenerateAccess;
using Application.Ecom.Authentication.Commands.LoginUser;
using Application.Ecom.Authentication.Commands.RegisterCustomer;
using Application.Ecom.Authentication.Commands.RegisterSeller;
using Application.Ecom.Authentication.Commands.VerifyOTP;
using Application.Ecom.OrderOperations.Commands.PlaceOrder;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;

namespace Application.Ecom.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly(),
                    typeof(RegisterCustomerCommandHandler).Assembly,
                    typeof(RegisterSellerCommandHandler).Assembly,
                    typeof(LoginUserCommandHandler).Assembly,
                    typeof(VerifyOtpCommandHandler).Assembly,
                    typeof(GenerateAccessTokenCommandHandler).Assembly,
                    typeof(PlaceOrderCommandHandler).Assembly
                    ));

            return services;
        }

        public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
        {
            
            services.AddValidatorsFromAssemblyContaining<PlaceOrderCommandValidation>();
            

            return services;
        }
    }
}
