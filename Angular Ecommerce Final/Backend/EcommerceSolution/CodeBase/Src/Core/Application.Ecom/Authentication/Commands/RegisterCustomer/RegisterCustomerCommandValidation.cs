using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ecom.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidation()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password).WithMessage("Password confirmation does not match.");
        }
    }
}