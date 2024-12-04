using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Dtos.ReviewerDtos;

namespace PokemonReviewApp.Services.ReviewerServices
{
    public interface IReviewerService
    {
        ICollection<ReviewerDto> GetReviewers();
        ReviewerDto GetReviewerById(int reviewerId);
        ICollection<ReviewDto> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        ReviewerDto CreateReviewer(ReviewerCreateDto reviewerCreateDto);
        bool UpdateReviewer(ReviewerUpdateDto reviewerUpdateDto);
        bool DeleteReviewer(int reviewerId);
    }
}
