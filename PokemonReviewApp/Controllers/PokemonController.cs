using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.PokemonServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/poke")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonService.GetPokemons();
            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        public IActionResult GetPokemonById(int id)
        {
            var pokemon = _pokemonService.GetPokemonById(id);
            if (pokemon == null)
                return NotFound();
            return Ok(pokemon);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetPokemonByName(string name)
        {
            var pokemon = _pokemonService.GetPokemonByName(name);
            if (pokemon == null)
                return NotFound();
            return Ok(pokemon);
        }

        [HttpGet("{id}/rating")]
        public IActionResult GetPokemonRating(int id)
        {
            if (!_pokemonService.PokemonExists(id))
                return NotFound();

            var rating = _pokemonService.GetPokemonRating(id);
            return Ok(rating);
        }
        [HttpPost]
        public IActionResult CreatePokemon(int ownerId, int categoryId, [FromBody] PokemonCreateDto pokemonCreateDto)
        {
            if (pokemonCreateDto == null)
                return BadRequest(ModelState);

            // Pokémon'ı oluşturmak için servisi çağır
            var created = _pokemonService.CreatePokemon(ownerId, categoryId, pokemonCreateDto);
            if (!created)
            {
                ModelState.AddModelError("", "Something went wrong while saving the pokemon.");
                return StatusCode(500, ModelState);
            }

            return Ok("Pokemon successfully created.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePokemon(int id, int ownerId, int categoryId, [FromBody] PokemonUpdateDto pokemonUpdateDto)
        {
            if (pokemonUpdateDto == null || id != pokemonUpdateDto.Id)
                return BadRequest(ModelState);

            if (!_pokemonService.PokemonExists(id))
                return NotFound();

            // Pokémon'u güncellemek için servisi çağır
            var updated = _pokemonService.UpdatePokemon(ownerId, categoryId, pokemonUpdateDto);
            if (!updated)
            {
                ModelState.AddModelError("", "Something went wrong while updating the pokemon.");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePokemon(int id)
        {
            if (!_pokemonService.PokemonExists(id))
                return NotFound();

            // Pokémon'u silmek için servisi çağır
            var deleted = _pokemonService.DeletePokemon(id);
            if (!deleted)
            {
                ModelState.AddModelError("", "Something went wrong while deleting the pokemon.");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // 204 No Content
        }

    }
}
