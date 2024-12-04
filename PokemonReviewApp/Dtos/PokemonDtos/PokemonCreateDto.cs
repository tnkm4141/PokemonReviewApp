namespace PokemonReviewApp.Dtos.PokemonDtos
{
    public class PokemonCreateDto
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        // CategoryId ekliyoruz
        public int CategoryId { get; set; }
    }
}
