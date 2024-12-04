using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.CategoryServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
       
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
        [HttpGet("{categoryId}/pokemons")]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            // Servis katmanından kategoriye ait Pokemonları alıyoruz
            var pokemons = _categoryService.GetPokemonsByCategory(categoryId);

            // Eğer bu kategoriye ait Pokemon yoksa 404 döndürüyoruz
            if (pokemons == null || !pokemons.Any())
            {
                return NotFound($"No Pokemons found for Category ID {categoryId}");
            }

            // Pokemon DTO'larını başarılı şekilde döndürüyoruz
            return Ok(pokemons);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            if (categoryCreateDto == null)
                return BadRequest();

            if (_categoryService.CreateCategory(categoryCreateDto))
                return Ok();
            else
                return StatusCode(500, "Error creating category");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null || id != categoryUpdateDto.Id)
                return BadRequest();

            var categoryExists = _categoryService.CategoryExists(id);
            if (!categoryExists)
                return NotFound();

            if (_categoryService.UpdateCategory(categoryUpdateDto))
                return Ok();
            else
                return StatusCode(500, "Error updating category");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryExists = _categoryService.CategoryExists(id);
            if (!categoryExists)
                return NotFound();

            if (_categoryService.DeleteCategory(id))
                return Ok();
            else
                return StatusCode(500, "Error deleting category");
        }



    }
}
