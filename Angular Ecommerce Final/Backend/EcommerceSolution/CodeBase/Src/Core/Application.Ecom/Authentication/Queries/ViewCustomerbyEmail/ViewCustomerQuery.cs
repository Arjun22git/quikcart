using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Queries.ViewCustomerbyEmail
{
    public class ViewCustomerQuery: IRequest<ViewCustomerQueryResponse>
    {
        public string Email { get; set; }
    }
}
