using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.CartOperations.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandValidation : AbstractValidator<UpdateCartItemCommand>
    {
        public UpdateCartItemCommandValidation()
        {
            RuleFor(command => command.CartItemId)
                .NotEmpty().WithMessage("Cart Item ID is required.");

            RuleFor(command => command.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
