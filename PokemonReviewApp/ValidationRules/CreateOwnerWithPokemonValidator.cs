using FluentValidation;
using PokemonReviewApp.Dtos.OwnerDtos;

namespace PokemonReviewApp.ValidationRules
{
    public class CreateOwnerWithPokemonValidator:AbstractValidator<CreateOwnerWithPokemonDto>
    {
        public CreateOwnerWithPokemonValidator() {
            RuleFor(dto => dto.Owner)
               .NotNull().SetValidator(new OwnerCreateValidatior()).WithMessage("Owner cannot be null.");

            RuleFor(dto => dto.Pokemon)
                .NotNull().SetValidator(new PokemonCreateValidator()).WithMessage("Pokemon cannot be null.");
        }
    }
}
