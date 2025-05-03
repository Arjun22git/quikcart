using FluentValidation;

namespace Application.Ecom.OrderOperations.Commands.PlaceOrder
{
    public class PlaceOrderCommandValidation : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidation()
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(command => command.OrderItems)
                .NotEmpty().WithMessage("At least one order item is required.")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("At least one order item is required.");

            RuleFor(command => command.OrderItems.Count)
                .NotEmpty().WithMessage("Min Quantity is 1")
                .LessThanOrEqualTo(5).WithMessage("Max quantity is 5");

            RuleFor(command => command.ShippingAddress)
                .NotEmpty().WithMessage("Shipping address is required.");
        }
    }
}
