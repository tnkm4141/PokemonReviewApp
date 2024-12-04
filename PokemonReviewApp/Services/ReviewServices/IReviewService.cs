using PokemonReviewApp.Dtos.ReviewDtos;

namespace PokemonReviewApp.Services.ReviewServices
{
    public interface IReviewService
    {
        ICollection<ReviewDto> GetReviews();
        ReviewDto GetReviewById(int reviewId);
        ICollection<ReviewDto> GetReviewsOfPokemon(int pokeId);
        bool ReviewExists(int reviewId);

        bool CreateReview(ReviewCreateDto reviewCreateDto);
        bool UpdateReview(ReviewUpdateDto reviewUpdateDto);
        bool DeleteReview(int reviewId);
        bool DeleteReviews(List<int> reviewIds); // Yeni metot eklendi
    }
}
