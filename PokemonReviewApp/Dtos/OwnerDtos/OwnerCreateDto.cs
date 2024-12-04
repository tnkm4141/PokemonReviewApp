using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dtos.OwnerDtos
{
    public class OwnerCreateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }

        // DTO'da CountryId alanı kullanılıyor
        public int CountryId { get; set; }
    }
}
