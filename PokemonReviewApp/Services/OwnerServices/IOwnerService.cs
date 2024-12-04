using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;

namespace PokemonReviewApp.Services.OwnerServices
{
    public interface IOwnerService
    {
        ICollection<OwnerDto> GetOwners();
        OwnerDto GetOwnerById(int ownerId);
        ICollection<OwnerDto> GetOwnersOfPokemon(int pokeId);
        ICollection<PokemonDto> GetPokemonsByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(OwnerCreateDto ownerCreateDto);
        public bool CreateOwnerWithPokemon(OwnerCreateDto ownerCreateDto, PokemonCreateDto pokemonCreateDto);
        bool UpdateOwner(int ownerId, OwnerUpdateDto ownerUpdateDto);
        bool DeleteOwner(int ownerId);

    }
}
