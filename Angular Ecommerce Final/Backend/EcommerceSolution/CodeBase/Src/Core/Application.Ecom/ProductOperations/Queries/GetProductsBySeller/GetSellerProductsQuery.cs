using Application.Ecom.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Queries.GetProductsBySeller
{
    public class GetSellerProductsQuery : IRequest<List<ProductDto>>
    {
        public Guid SellerId { get; set; }
    }
}
