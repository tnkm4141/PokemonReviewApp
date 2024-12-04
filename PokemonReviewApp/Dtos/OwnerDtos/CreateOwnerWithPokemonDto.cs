using PokemonReviewApp.Dtos.PokemonDtos;
using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dtos.OwnerDtos
{
    public class CreateOwnerWithPokemonDto
    {
       
        public OwnerCreateDto Owner { get; set; }
      
        public PokemonCreateDto Pokemon { get; set; }
    }
}
