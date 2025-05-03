using Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<CommandResponse>
    {
        public Guid OrderId { get; set; }
        public bool IsDelivered { get; set; }
        public string OrderStatus { get; set; } 
        public DateOnly DeliveryDate { get; set; } 
    }
}
