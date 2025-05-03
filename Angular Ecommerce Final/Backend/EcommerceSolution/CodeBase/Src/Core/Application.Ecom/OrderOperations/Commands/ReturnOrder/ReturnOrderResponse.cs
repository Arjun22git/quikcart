using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.ReturnOrder
{
    public class ReturnOrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
