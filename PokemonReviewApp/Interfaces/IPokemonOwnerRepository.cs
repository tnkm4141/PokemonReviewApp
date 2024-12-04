using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonOwnerRepository
    {
        bool AddPokemonOwner(PokemonOwner pokemonOwner); // PokemonOwner kaydetme
        bool RemovePokemonOwner(PokemonOwner pokemonOwner); // PokemonOwner silme
        ICollection<PokemonOwner> GetPokemonOwnersByOwner(int ownerId); // Owner'a ait Pokemon'ları listeleme
        ICollection<PokemonOwner> GetPokemonOwnersByPokemon(int pokemonId); // Pokemon'a ait Owner'ları listeleme
        bool CreatePokemonOwner(PokemonOwner pokemonOwner);
        bool save(); // Değişiklikleri kaydetme
    }
}
