using AutoMapper;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.PokemonServices
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonService(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        public ICollection<PokemonDto> GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();
            return _mapper.Map<ICollection<PokemonDto>>(pokemons);
        }

        public PokemonDto GetPokemonById(int id)
        {
            var pokemon = _pokemonRepository.GetPokemon(id);
            return _mapper.Map<PokemonDto>(pokemon);
        }

        public PokemonDto GetPokemonByName(string name)
        {
            var pokemon = _pokemonRepository.GetPokemon(name);
            return _mapper.Map<PokemonDto>(pokemon);
        }

        public decimal GetPokemonRating(int pokeId)
        {
            return _pokemonRepository.GetPokemonRating(pokeId);
        }

        public bool PokemonExists(int pokeId)
        {
            return _pokemonRepository.PokemonExists(pokeId);
        }
        public bool CreatePokemon(int ownerId, int categoryId, PokemonCreateDto pokemonCreateDto)
        {
            // DTO'dan Pokemon modeline dönüştürme
            var pokemon = _mapper.Map<Pokemon>(pokemonCreateDto);
            return _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon);
        }

        public bool UpdatePokemon(int ownerId, int categoryId, PokemonUpdateDto pokemonUpdateDto)
        {
            // DTO'dan Pokemon modeline dönüştürme
            var pokemon = _mapper.Map<Pokemon>(pokemonUpdateDto);
            return _pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemon);
        }

        public bool DeletePokemon(int pokeId)
        {
            return _pokemonRepository.DeletePokemon(new Pokemon { Id = pokeId });
        }
    }
}
