using Application.Ecom.Dtos;
using MediatR;

namespace Application.Ecom.ProductOperations.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PaginatedProductResponse>
    {
        public int PageNumber { get; set; } = 1; // Default to the first page
        public int PageSize { get; set; } = 10; // Default number of items per page
    }

}
