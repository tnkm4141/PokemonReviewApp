using PokemonReviewApp.Data;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
           
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(p => p.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
         return _context.PokemonOwners.Where(p=>p.Pokemon.Id== pokeId).Select(o=>o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p=>p.Owner.Id == ownerId).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(p=>p.Id == ownerId);
        }
        public bool CreateOwner(Owner owner)
        {
            // Owner nesnesini veritabanına ekliyoruz
            _context.Owners.Add(owner);

            // Değişiklikleri kaydetmek için SaveChanges kullanıyoruz
            return _context.SaveChanges() > 0;
        }
        public bool UpdateOwner(Owner owner)
        {
            // Mevcut owner'ı buluyoruz
            var existingOwner = GetOwner(owner.Id);
            if (existingOwner == null)
            {
                return false; // Güncellenmek istenen owner bulunamadı
            }

            // Güncellenmek istenen owner'ın mevcut verilerini güncelliyoruz
            existingOwner.FirstName = owner.FirstName; 
            existingOwner.LastName = owner.LastName; 
            existingOwner.Gym = owner.Gym; 
            //existingOwner.Country = owner.Country; 

            // Değişiklikleri kaydediyoruz
            return _context.SaveChanges() > 0;
        }

        public bool DeleteOwner(int ownerId)
        {
            // Silmek istediğimiz owner'ı buluyoruz
            var ownerToDelete = GetOwner(ownerId);
            if (ownerToDelete == null)
            {
                return false; // Silinmek istenen owner bulunamadı
            }

            _context.Owners.Remove(ownerToDelete); // Owner'ı sil
            return _context.SaveChanges() > 0; // Değişiklikleri kaydet
        }
    }
}
