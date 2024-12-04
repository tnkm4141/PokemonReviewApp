using AutoMapper;
using PokemonReviewApp.Dtos.CountryDtos;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.CountryServices
{
    public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;

        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public bool CountryExists(int id)
        {
            return _countryRepository.CountryExists(id);
        }

        public ICollection<CountryDto> GetCountries()
        {
            var countries = _countryRepository.GetCountries();
            return _mapper.Map<ICollection<CountryDto>>(countries);
        }

        public CountryDto GetCountryById(int id)
        {
            var country = _countryRepository.GetCountry(id);
            return _mapper.Map<CountryDto>(country);
        }

        public CountryDto GetCountryByOwner(int ownerId)
        {
            var country = _countryRepository.GetCountryByOwner(ownerId);
            return _mapper.Map<CountryDto>(country);
        }

        
        public bool CreateCountry(CountryCreateDto countryCreateDto)
        {
            var country = _mapper.Map<Country>(countryCreateDto);

            if (country == null)
            {
                return false;
            }

            return _countryRepository.CreateCountry(country);
        }

        public bool UpdateCountry(CountryUpdateDto countryUpdateDto)
        {
            var country = _mapper.Map<Country>(countryUpdateDto);

            if (country == null)
            {
                return false;
            }

            return _countryRepository.UpdateCountry(country);
        }

        public bool DeleteCountry(int id)
        {
            var country = _countryRepository.GetCountry(id);

            if (country == null)
            {
                return false;
            }

            return _countryRepository.DeleteCountry(country);
        }

        public ICollection<OwnerDto> GetOwnerFromACountry(int countryId)
        {
            var owners = _countryRepository.GetOwnersFromACountry(countryId);
            return _mapper.Map<ICollection<OwnerDto>>(owners);
        }
    }
}
