using Application.Ecom.Dtos;

namespace Application.Ecom.ProductOperations.Queries.GetAllProducts
{
    public class PaginatedProductResponse 
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}