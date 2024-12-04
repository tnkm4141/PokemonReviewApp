using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonOwnerRepository : IPokemonOwnerRepository
    {
        private readonly DataContext _context; // Veritabanı bağlantısı

        public PokemonOwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool AddPokemonOwner(PokemonOwner pokemonOwner)
        {
            _context.Add(pokemonOwner);
            return save();
        }

        public bool RemovePokemonOwner(PokemonOwner pokemonOwner)
        {
            _context.Remove(pokemonOwner);
            return save();
        }

        public ICollection<PokemonOwner> GetPokemonOwnersByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).ToList();
        }

        public ICollection<PokemonOwner> GetPokemonOwnersByPokemon(int pokemonId)
        {
            return _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).ToList();
        }

        public bool save()
        {
            return _context.SaveChanges() > 0;
        }
        public bool CreatePokemonOwner(PokemonOwner pokemonOwner)
        {
            // PokemonOwner ekleme işlemleri
            _context.PokemonOwners.Add(pokemonOwner);
            return _context.SaveChanges() > 0; // Başarılı ise true döner
        }


    }
}
