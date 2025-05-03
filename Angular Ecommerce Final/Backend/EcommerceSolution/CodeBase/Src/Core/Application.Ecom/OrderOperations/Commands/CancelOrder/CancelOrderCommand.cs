using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.CancelOrder
{
    public class CancelOrderCommand :IRequest<CancelOrderResponse>
    {
        public Guid OrderId { get; set; }
    }
}
