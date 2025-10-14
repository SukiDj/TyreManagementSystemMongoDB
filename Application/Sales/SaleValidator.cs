using Domain;
using FluentValidation;

namespace Application.Sales
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.PricePerUnit).NotEmpty();
            RuleFor(x => x.QuantitySold).NotEmpty();
            RuleFor(x => x.SaleDate).NotEmpty();
            RuleFor(x => x.TargetMarket).NotEmpty();
            RuleFor(x => x.UnitOfMeasure).NotEmpty();
        }
    }
}