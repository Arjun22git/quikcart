using Application.Ecom.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Mapper
{
    public static class MapOrder
    {
        public static PlaceOrderDto MapCartToOrder(Guid customerId, List<CartItemDto> cartItems, string shippingAddress)
        {
            var orderDto = new PlaceOrderDto
            {
                CustomerId = customerId,
                ShippingAddress = shippingAddress,
                OrderItems = new List<OrderItemDto>()
            };

            foreach (var cartItem in cartItems)
            {
                orderDto.OrderItems.Add(new OrderItemDto
                {
                    ProductId = cartItem.ProductId, 
                    Quantity = cartItem.Quantity
                });
            }

            return orderDto;
        }
    }

}
