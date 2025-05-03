using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.RegisterSeller
{
    public class RegisterSellerCommand : IRequest<RegisterSellerResponse>
    {
       
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }       
        public string StoreName { get; set; } 
        public string StoreDescription { get; set; } 
    }
}
