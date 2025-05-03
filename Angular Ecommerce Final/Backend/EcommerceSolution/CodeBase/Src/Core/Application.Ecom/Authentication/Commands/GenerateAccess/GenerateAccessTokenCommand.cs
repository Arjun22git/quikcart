using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.GenerateAccess
{
    public class GenerateAccessTokenCommand : IRequest<string>
    {
        public string RefreshToken { get; set; }
    }
}
