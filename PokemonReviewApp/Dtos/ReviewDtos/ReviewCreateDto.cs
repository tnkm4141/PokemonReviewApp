namespace PokemonReviewApp.Dtos.ReviewDtos
{
    public class ReviewCreateDto
    {
        public string Title { get; set; } 
        public string Text { get; set; } 
        public int Rating { get; set; } 
        public int PokemonId { get; set; } 
    }
}
