using FluentValidation;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.OwnerServices;

namespace PokemonReviewApp.ValidationRules
{
    public class OwnerCreateValidatior:AbstractValidator<OwnerCreateDto>
    {
        
        public OwnerCreateValidatior()
        {
           

            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Owner Adını Boş Geçemezsiniz.")
            .MinimumLength(3).WithMessage("Lütfen En Az 3 Karakter Girişi Yapın.")
            .MaximumLength(10).WithMessage("Lütfen 10 Karakterden Fazla Girişi Yapmayın.")
            .Must(name => !IsAllDigits(name)).WithMessage("Owner adı sadece sayılardan oluşamaz.");//isim sadece sayı olamaz

            RuleFor(x => x.LastName)
             .NotEmpty().WithMessage("Owner Soy Adını Boş Geçemezsiniz")
             .MinimumLength(3).WithMessage("Lütfen En Az 3 Karakter Girişi Yapın.")
             .MaximumLength(10).WithMessage("Lütfen 10 Karakterden Fazla Girişi Yapmayın.");
     
            RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId  Boş Geçemezsiniz.");

          
        }
        // Sadece sayı içeren string olup olmadığını kontrol eden metod
        private bool IsAllDigits(string value)
        {
            return value.All(char.IsDigit);
        }
        
       
    }
}
