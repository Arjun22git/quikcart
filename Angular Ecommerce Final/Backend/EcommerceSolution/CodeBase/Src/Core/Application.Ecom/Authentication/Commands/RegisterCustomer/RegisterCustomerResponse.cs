using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Email { get; set; } // Optional: include email if needed for further actions
    }

}
