using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.VerifyOTP
{
    public class VerifyOtpCommand : IRequest<VerifyOtpResponse>
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

}
