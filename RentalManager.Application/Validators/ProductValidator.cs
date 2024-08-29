using FluentValidation;
using RentalManager.Domain.Models;

namespace RentalManager.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.ProductName)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .Length(2, 50).WithMessage("Nazwa musi mieć między 2 a 50 znaków.");
            RuleFor(product => product.Description)
                .NotEmpty().WithMessage("Opis jest wymagany").Length(0, 500).WithMessage("Opis nie może przekraczać 500 znaków.");
            RuleFor(product => product.Price).NotEmpty().WithMessage("Cena jest wymagana.").GreaterThanOrEqualTo(0).WithMessage("Cena nie może być ujemna.");
            RuleFor(product => product.QuantityInStock).NotEmpty().WithMessage("Ilość w magazynie jest wymagana.").GreaterThanOrEqualTo(0).WithMessage("Ilość w magazynie nie może być ujemna.");
        }
    }
}
