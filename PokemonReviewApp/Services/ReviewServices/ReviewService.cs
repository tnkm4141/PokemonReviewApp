using AutoMapper;
using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IPokemonRepository pokemonRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
        }

        public ICollection<ReviewDto> GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();
            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }

        public ReviewDto GetReviewById(int reviewId)
        {
            var review = _reviewRepository.GetReview(reviewId);
            return _mapper.Map<ReviewDto>(review);
        }

        public ICollection<ReviewDto> GetReviewsOfPokemon(int pokeId)
        {
            var reviews = _reviewRepository.GetReviewsOfAPokemon(pokeId);
            return _mapper.Map<ICollection<ReviewDto>>(reviews);
        }

        public bool ReviewExists(int reviewId)
        {
            return _reviewRepository.ReviewExsist(reviewId);
        }
        public bool CreateReview(ReviewCreateDto reviewCreateDto)
        {
            // Öncelikle PokemonId'nin varlığını kontrol et
            if (!_pokemonRepository.PokemonExists(reviewCreateDto.PokemonId))
            {
                throw new ArgumentException("The specified Pokemon does not exist.");
            }

            // DTO'dan Review modeline dönüştürme
            var review = _mapper.Map<Review>(reviewCreateDto);

            // Repository'de incelemeyi oluşturma
            return _reviewRepository.CreateReview(review);
        }

        public bool UpdateReview(ReviewUpdateDto reviewUpdateDto)
        {
            // DTO'dan Review modeline dönüştürme
            var review = _mapper.Map<Review>(reviewUpdateDto);

            // Repository'de incelemeyi güncelleme
            return _reviewRepository.UpdateReview(review);
        }

        public bool DeleteReview(int reviewId)
        {
            // İncelemeyi ID ile bul ve sil
            var review = _reviewRepository.GetReview(reviewId);
            if (review == null)
            {
                return false; // Eğer inceleme bulunamazsa, silme işlemi başarısız
            }
            return _reviewRepository.DeleteReview(review);
        }

        public bool DeleteReviews(List<int> reviewIds)
        {
            // Belirtilen incelemeleri silme
            var reviewsToDelete = new List<Review>();
            foreach (var reviewId in reviewIds)
            {
                var review = _reviewRepository.GetReview(reviewId);
                if (review != null)
                {
                    reviewsToDelete.Add(review);
                }
            }
            return _reviewRepository.DeleteReviews(reviewsToDelete);
        }
    }
}
