using FluentValidation;
using PokemonReviewApp.Dtos.PokemonDtos;

namespace PokemonReviewApp.ValidationRules
{
    public class PokemonCreateValidator:AbstractValidator<PokemonCreateDto>
    {
        public PokemonCreateValidator() 
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Pokemon Adını Boş Geçemezsiniz.");
            RuleFor(x=>x.Name).MinimumLength(3).WithMessage("Lütfen En Az 3 Karakter Giriniz.");
            RuleFor(x=>x.Name).MaximumLength(10).WithMessage("Lütfen 10 Karakterden Fazla Girmeyiniz.");

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryID Boş Geçemezsiniz.");
            
        }
    }
}
