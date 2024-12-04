using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.ReviewServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _reviewService.GetReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetReviewsOfPokemon(int pokeId)
        {
            var reviews = _reviewService.GetReviewsOfPokemon(pokeId);
            if (reviews == null || !reviews.Any())
                return NotFound();
            return Ok(reviews);
        }
        [HttpPost]
        public IActionResult CreateReview([FromBody] ReviewCreateDto reviewCreateDto)
        {
            if (reviewCreateDto == null)
                return BadRequest(ModelState);

            // Yeni incelemeyi oluştur
            var created = _reviewService.CreateReview(reviewCreateDto);
            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving the review.");
                return StatusCode(500, ModelState);
            }

            return Ok("Review successfully created.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            if (reviewUpdateDto == null || reviewUpdateDto.Id != id)
                return BadRequest(ModelState);

            // İncelemeyi güncelle
            var updated = _reviewService.UpdateReview(reviewUpdateDto);
            if (!updated)
            {
                ModelState.AddModelError("", "Something went wrong while updating the review.");
                return StatusCode(500, ModelState);
            }

            return Ok("Review successfully updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            // İncelemeyi sil
            var deleted = _reviewService.DeleteReview(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Something went wrong while deleting the review.");
                return StatusCode(500, ModelState);
            }

            return Ok("Review successfully deleted.");
        }
        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteReviews([FromBody] List<int> reviewIds)
        {
            if (reviewIds == null || !reviewIds.Any())
                return BadRequest("Review IDs cannot be null or empty.");

            // Birden fazla incelemeyi sil
            var deleted = _reviewService.DeleteReviews(reviewIds);
            if (!deleted)
            {
                ModelState.AddModelError("", "Something went wrong while deleting the reviews.");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviews successfully deleted.");
        }

    }
}
