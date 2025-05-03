using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtpAsync(string email);
        Task SendOtpEmailAsync(string email, string otp);
        Task<bool> VerifyOtpAsync(string email, string otp);
    }

}
