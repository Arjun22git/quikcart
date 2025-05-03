using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.LoginUser
{
    public class LoginUserCommandResponse
    {
        public string Access {  get; set; }

        public string Refresh {  get; set; }

        public IEnumerable<string> Role { get; set; }
    }
}
