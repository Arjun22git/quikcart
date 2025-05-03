using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember  {  get; set; }
    }
}
