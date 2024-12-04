using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dtos.CountryDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services.CountryServices;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _countryService.GetCountries();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public IActionResult GetCountryById(int id)
        {
            var country = _countryService.GetCountryById(id);
            if (country == null)
                return NotFound();
            return Ok(country);
        }

        /*[HttpGet("owner/{ownerId}")]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _countryService.GetCountryByOwner(ownerId);
            if (country == null)
                return NotFound();
            return Ok(country);
        }

        [HttpGet("{id}/owners")]
        public IActionResult GetOwnerFromCountry(int id)
        {
            var owners = _countryService.GetOwnerFromACountry(id);
            if (owners == null || !owners.Any())
                return NotFound();
            return Ok(owners);
        }*/
        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryCreateDto countryCreateDto)
        {
            if (countryCreateDto == null)
                return BadRequest("Country data is required.");

            if (_countryService.CreateCountry(countryCreateDto))
                return Ok("Country created successfully.");
            else
                return StatusCode(500, "Error creating the country.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCountry(int id, [FromBody] CountryUpdateDto countryUpdateDto)
        {
            if (countryUpdateDto == null || id != countryUpdateDto.Id)
                return BadRequest("Invalid data.");

            if (!_countryService.CountryExists(id))
                return NotFound("Country not found.");

            if (_countryService.UpdateCountry(countryUpdateDto))
                return Ok("Country updated successfully.");
            else
                return StatusCode(500, "Error updating the country.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            if (!_countryService.CountryExists(id))
                return NotFound("Country not found.");

            if (_countryService.DeleteCountry(id))
                return Ok("Country deleted successfully.");
            else
                return StatusCode(500, "Error deleting the country.");
        }
    }
}
