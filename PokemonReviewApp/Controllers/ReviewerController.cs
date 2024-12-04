using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.ReviewDtos;
using PokemonReviewApp.Dtos.ReviewerDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.ReviewerServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            var reviewers = _reviewerService.GetReviewers();
            return Ok(reviewers);
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewerById(int id)
        {
            var reviewer = _reviewerService.GetReviewerById(id);
            if (reviewer == null)
                return NotFound();
            return Ok(reviewer);
        }

        [HttpGet("{id}/reviews")]
        public IActionResult GetReviewsByReviewer(int id)
        {
            if (!_reviewerService.ReviewerExists(id))
                return NotFound();

            var reviews = _reviewerService.GetReviewsByReviewer(id);
            return Ok(reviews);
        }
        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerCreateDto reviewerCreateDto)
        {
            if (reviewerCreateDto == null)
                return BadRequest(ModelState);

            var createdReviewer = _reviewerService.CreateReviewer(reviewerCreateDto);
            return CreatedAtAction("GetReviewerById", new { id = createdReviewer.Id }, createdReviewer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReviewer(int id, [FromBody] ReviewerUpdateDto reviewerUpdateDto)
        {
            if (reviewerUpdateDto == null || id != reviewerUpdateDto.Id)
                return BadRequest(ModelState);

            if (!_reviewerService.ReviewerExists(id))
                return NotFound();

            if (!_reviewerService.UpdateReviewer(reviewerUpdateDto))
                return StatusCode(500, "Something went wrong while updating the reviewer");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReviewer(int id)
        {
            if (!_reviewerService.ReviewerExists(id))
                return NotFound();

            if (!_reviewerService.DeleteReviewer(id))
                return StatusCode(500, "Something went wrong while deleting the reviewer");

            return NoContent();
        }


    }
}
