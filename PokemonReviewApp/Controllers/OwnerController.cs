using FluentValidation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.OwnerServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerService _ownerService;
        private readonly IValidator<CreateOwnerWithPokemonDto> _validator;
        private readonly ILogger<OwnerController> _logger;

        public OwnerController(IOwnerService ownerService, IValidator<CreateOwnerWithPokemonDto> validator, ILogger<OwnerController> logger)
        {
            _ownerService = ownerService;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetOwners()
        {
            var owners = _ownerService.GetOwners();
            return Ok(owners);
        }

        [HttpGet("{id}")]
        public IActionResult GetOwnerById(int id)
        {
            var owner = _ownerService.GetOwnerById(id);
            if (owner == null)
                return NotFound();
            return Ok(owner);
        }

        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetOwnersOfPokemon(int pokeId)
        {
            var owners = _ownerService.GetOwnersOfPokemon(pokeId);
            if (owners == null || !owners.Any())
                return NotFound();
            return Ok(owners);
        }

        [HttpGet("{id}/pokemons")]
        public IActionResult GetPokemonsByOwner(int id)
        {
            var pokemons = _ownerService.GetPokemonsByOwner(id);
            if (pokemons == null || !pokemons.Any())
                return NotFound();
            return Ok(pokemons);
        }
        [HttpPost]
        public IActionResult CreateOwnerWithPokemon([FromBody] CreateOwnerWithPokemonDto ownerWithPokemonCreateDto)
        {

            _logger.LogInformation("Owner creation started.");

            if (ownerWithPokemonCreateDto == null)
            {
                _logger.LogWarning("CreateOwnerWithPokemon called with null DTO.");
                return BadRequest(new { IsValid = false, Message = "Invalid DTO received." });
            }


            // FluentValidation kullanarak doğrula
            var validationResult = _validator.Validate(ownerWithPokemonCreateDto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for CreateOwnerWithPokemon DTO.");

                // Hata mesajlarını Owner ve Pokemon gruplarına ayır
                var ownerErrors = validationResult.Errors
                    .Where(e => e.PropertyName.StartsWith("Owner"))
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
                    .ToList();

                var pokemonErrors = validationResult.Errors
                    .Where(e => e.PropertyName.StartsWith("Pokemon"))
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
                    .ToList();

                // Hata mesajlarını logla
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogWarning($"Validation error: Property {error.PropertyName}, Error: {error.ErrorMessage}");
                }

                // Gruplandırılmış hata mesajları ile response döndür
                return BadRequest(new
                {
                    IsValid = false,
                    Errors = new
                    {
                        Owner = ownerErrors,
                        Pokemon = pokemonErrors
                    }
                });
            }


            // Owner ve Pokemon oluştur
            var result = _ownerService.CreateOwnerWithPokemon(ownerWithPokemonCreateDto.Owner, ownerWithPokemonCreateDto.Pokemon);


            if (!result)
            {
                _logger.LogError("Something went wrong while saving the owner and pokemon.");
                return StatusCode(500, new { IsValid = false, Message = "Something went wrong while saving the owner and pokemon." });
            }

            _logger.LogInformation("Owner and Pokemon successfully created.");
            return Ok(new { IsValid = true, Message = "Owner and Pokemon successfully created." });

        }


        /* [HttpPost]
         public IActionResult CreateOwner([FromBody] OwnerCreateDto ownerCreateDto)
         {
             if (ownerCreateDto == null)
                 return BadRequest(ModelState);

             // Örnek: Aynı isimde bir owner olup olmadığını kontrol etmek için
             var existingOwner = _ownerService.GetOwners()
                 .FirstOrDefault(o => o.FirstName.Trim().ToUpper() == ownerCreateDto.FirstName.Trim().ToUpper()
                                      && o.LastName.Trim().ToUpper() == ownerCreateDto.LastName.Trim().ToUpper());

             if (existingOwner != null)
             {
                 ModelState.AddModelError("", "Owner already exists.");
                 return StatusCode(422, ModelState);
             }

             // Eğer hata yoksa, owner'ı oluşturmak için devam edin
             var newOwner = _ownerService.CreateOwner(ownerCreateDto);

             if (!newOwner)
             {
                 ModelState.AddModelError("", "Something went wrong while saving the owner.");
                 return StatusCode(500, ModelState);
             }

             return Ok("Owner successfully created.");
         }*/
        [HttpPut("{id}")]
        public IActionResult UpdateOwner(int id, [FromBody] OwnerUpdateDto ownerUpdateDto)
        {
            if (ownerUpdateDto == null)
                return BadRequest(ModelState);

            var updated = _ownerService.UpdateOwner(id, ownerUpdateDto);
            if (!updated)
            {
                ModelState.AddModelError("", "Something went wrong while updating the owner.");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOwner(int id)
        {
            var deleted = _ownerService.DeleteOwner(id);
            if (!deleted)
            {
                return NotFound($"Owner with ID {id} not found.");
            }

            return NoContent(); // 204 No Content
        }

    }
}
