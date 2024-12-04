using AutoMapper;
using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Dtos.ReviewerDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.ReviewerServices
{
    public class ReviewerService : IReviewerService
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerService(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        public ICollection<ReviewerDto> GetReviewers()
        {
            var reviewers = _reviewerRepository.GetReviewers();
            return _mapper.Map<ICollection<ReviewerDto>>(reviewers);
        }

        public ReviewerDto GetReviewerById(int reviewerId)
        {
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            return _mapper.Map<ReviewerDto>(reviewer);
        }

        public ICollection<ReviewDto> GetReviewsByReviewer(int reviewerId)
        {
            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);
            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _reviewerRepository.ReviewerExists(reviewerId);
        }
        public ReviewerDto CreateReviewer(ReviewerCreateDto reviewerCreateDto)
        {
            var reviewer = _mapper.Map<Reviewer>(reviewerCreateDto);
            var createdReviewer = _reviewerRepository.CreateReviewer(reviewer);
            return _mapper.Map<ReviewerDto>(createdReviewer);
        }

        public bool UpdateReviewer(ReviewerUpdateDto reviewerUpdateDto)
        {
            var reviewer = _mapper.Map<Reviewer>(reviewerUpdateDto);
            return _reviewerRepository.UpdateReviewer(reviewer);
        }

        public bool DeleteReviewer(int reviewerId)
        {
            // Reviewer nesnesini almak için ID'yi kullanarak veritabanından çekme
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (reviewer == null)
            {
                return false; // Eğer reviewer bulunamazsa, silme işlemi başarısız
            }
            return _reviewerRepository.DeleteReviewer(reviewer);
        }
    }
}
