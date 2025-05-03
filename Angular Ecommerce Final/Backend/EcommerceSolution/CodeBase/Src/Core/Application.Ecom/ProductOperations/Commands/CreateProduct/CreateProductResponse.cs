using Application.Ecom.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Commands.CreateProduct
{
    public class CreateProductResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ProductDto Product { get; set; }
    }
}
