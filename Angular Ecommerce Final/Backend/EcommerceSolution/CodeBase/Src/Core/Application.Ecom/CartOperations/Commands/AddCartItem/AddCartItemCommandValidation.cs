using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.AddCartItem
{
    public class AddCartItemCommandValidation : AbstractValidator<AddCartItemCommand>
    {
        public AddCartItemCommandValidation()
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(command => command.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(command => command.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
