using PokemonReviewApp.Dtos.PokemonDtos;

namespace PokemonReviewApp.Services.PokemonServices
{
    public interface IPokemonService
    {
        ICollection<PokemonDto> GetPokemons();
        PokemonDto GetPokemonById(int id);
        PokemonDto GetPokemonByName(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeId);
        bool CreatePokemon(int ownerId, int categoryId, PokemonCreateDto pokemonCreateDto);
        bool UpdatePokemon(int ownerId, int categoryId, PokemonUpdateDto pokemonUpdateDto);
        bool DeletePokemon(int pokeId);
    }
}
