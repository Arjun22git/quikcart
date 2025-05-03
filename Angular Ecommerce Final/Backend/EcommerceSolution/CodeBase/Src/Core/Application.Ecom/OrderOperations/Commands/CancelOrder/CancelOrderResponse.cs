using System.Text.Json.Serialization;

namespace Application.Ecom.OrderOperations.Commands.CancelOrder
{
    public class CancelOrderResponse
    {
        [JsonIgnore]
        public bool Success { get; set; }
        public Guid OrderId { get; set; }
        public string Message { get; set; }
    }
}