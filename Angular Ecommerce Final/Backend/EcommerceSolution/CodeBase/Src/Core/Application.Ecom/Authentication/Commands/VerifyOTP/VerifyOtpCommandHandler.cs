using Application.Ecom.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.VerifyOTP
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
    {
        private readonly IOtpService _otpService;
        private readonly IEcommerceDbContext _dbContext;

        public VerifyOtpCommandHandler(IOtpService otpService, IEcommerceDbContext dbContext)
        {
            _otpService = otpService;
            _dbContext = dbContext;
        }

        public async Task<VerifyOtpResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var isValid = await _otpService.VerifyOtpAsync(request.Email, request.Otp);

            if (isValid)
            {
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

                if (user != null)
                {
                    user.IsVerified = true;
                    await _dbContext.SaveChangesAsync();
                }

                return new VerifyOtpResponse
                {
                    Success = true,
                    Message = "OTP verified successfully."
                };
            }

            return new VerifyOtpResponse
            {
                Success = false,
                Message = "Invalid or expired OTP."
            };
        }
    }


}
