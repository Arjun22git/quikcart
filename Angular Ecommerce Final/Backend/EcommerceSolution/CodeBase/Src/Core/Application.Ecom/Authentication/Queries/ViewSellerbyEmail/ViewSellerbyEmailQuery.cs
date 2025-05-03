using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Queries.ViewSellerbyEmail
{
    public class ViewSellerbyEmailQuery : IRequest<object>
    {
        public string Email { get; set; }
    }
}
