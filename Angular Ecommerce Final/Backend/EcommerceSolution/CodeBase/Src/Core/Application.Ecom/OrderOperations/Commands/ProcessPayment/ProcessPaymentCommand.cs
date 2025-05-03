using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Ecom.Dtos;

namespace Application.Ecom.OrderOperations.Commands.ProcessPayment
{
    public class ProcessPaymentCommand : IRequest<PaymentDto>
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
