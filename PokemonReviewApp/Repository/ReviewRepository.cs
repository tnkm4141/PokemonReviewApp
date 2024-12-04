using AutoMapper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _context.Reviews.Where(r=>r.Pokemon.Id == pokeId).ToList();
        }

        public bool ReviewExsist(int reviewId)
        {
            return _context.Reviews.Any(r=>r.Id == reviewId);
        }
        public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            return save();
        }

        public bool UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            return save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return save();
        }
        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return save();
        }
        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
