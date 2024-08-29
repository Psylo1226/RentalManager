using FluentValidation;
using RentalManager.Domain.Models;

namespace RentalManager.Application.Validators;

    public class ProductTypeValidator : AbstractValidator<ProductType>
    {
        public ProductTypeValidator()
        {
            RuleFor(item => item.TypeName)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .Length(2, 50).WithMessage("Nazwa musi mieć między 2 a 50 znaków.");
            
        }
    }
