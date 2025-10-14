using Domain;
using FluentValidation;

namespace Application.Productions
{
    public class ProductionValidator : AbstractValidator<Production>
    {
        public ProductionValidator()
        {
            RuleFor(x => x.Machine).NotEmpty();
            RuleFor(x => x.Shift).NotEmpty();
            RuleFor(x => x.QuantityProduced).NotEmpty();
        }
    }
}