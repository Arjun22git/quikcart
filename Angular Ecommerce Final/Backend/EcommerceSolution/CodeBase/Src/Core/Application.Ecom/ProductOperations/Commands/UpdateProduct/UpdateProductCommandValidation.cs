using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.ProductOperations.Commands.UpdateProduct
{
    public class UpdateProductCommandValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidation()
        {
            RuleFor(command => command.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(1, 100).WithMessage("Product name must be between 1 and 100 characters.");

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage("Product description is required.")
                .Length(1, 500).WithMessage("Product description must be between 1 and 500 characters.");

            RuleFor(command => command.Category)
                .NotEmpty().WithMessage("Product category is required.");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than zero.");

            RuleFor(command => command.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Product quantity must be zero or greater.");
        }
    }
}
