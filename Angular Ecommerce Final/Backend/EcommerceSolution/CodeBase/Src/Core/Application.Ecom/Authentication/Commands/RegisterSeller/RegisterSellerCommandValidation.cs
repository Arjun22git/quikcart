using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Ecom.Authentication.Commands.RegisterSeller
{
    

    public class RegisterSellerCommandValidation : AbstractValidator<RegisterSellerCommand>
    {
        public RegisterSellerCommandValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Password confirmation does not match.");

            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Store name is required.")
                .MaximumLength(100).WithMessage("Store name cannot exceed 100 characters.");

            RuleFor(x => x.StoreDescription)
                .MaximumLength(500).WithMessage("Store description cannot exceed 500 characters.");
        }
    }
}
