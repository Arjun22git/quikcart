using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.ReturnOrder
{
    public class ReturnOrderCommand : IRequest<ReturnOrderResponse>
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }


    }
}
