using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Commands.PlaceOrder
{
    public class PlaceOrderCommandResponse
    {
        [JsonIgnore]
        public bool Success { get; set; }
        public string Message { get; set; }
        
        public Guid? OrderId { get; set; }
    }
}
