using PokemonReviewApp.Dtos.CountryDtos;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.CountryServices
{
    public interface ICountryService
    {
        ICollection<CountryDto> GetCountries();
        CountryDto GetCountryById(int id);
        CountryDto GetCountryByOwner(int ownerId);
        ICollection<OwnerDto> GetOwnerFromACountry(int countryId);
        bool CountryExists(int id);
        bool CreateCountry(CountryCreateDto countryCreateDto);
        bool UpdateCountry(CountryUpdateDto countryUpdateDto);
        bool DeleteCountry(int id);
    }
}
