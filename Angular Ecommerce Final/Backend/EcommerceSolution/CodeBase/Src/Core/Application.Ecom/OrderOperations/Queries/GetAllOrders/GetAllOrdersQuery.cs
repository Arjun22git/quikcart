using Application.Ecom.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.OrderOperations.Queries.GetAllOrders
{
    public class GetAllOrdersQuery  : IRequest<IEnumerable<OrderDto>>
    {
    }
}
