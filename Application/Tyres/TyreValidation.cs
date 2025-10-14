using Domain;
using FluentValidation;

namespace Application.Tyres
{
    public class TyreValidation : AbstractValidator<Tyre>
    {
        public TyreValidation()
        {
            RuleFor(x => x.Brand).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Size).NotEmpty();
            RuleFor(x => x.Type).NotEmpty();
        }
    }
}