using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Services.OwnerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using PokemonReviewApp.Interfaces;
using FluentValidationResult = FluentValidation.Results.ValidationResult;
using FluentValidationFailure = FluentValidation.Results.ValidationFailure;



namespace PokemonReviewTest
{
    public class OwnerControllerTest
    {
        private readonly Mock<IOwnerService> _mockOwnerService;
        private readonly Mock<IValidator<CreateOwnerWithPokemonDto>> _mockValidator;
        private readonly Mock<ILogger<OwnerController>> _mockLogger;
        private readonly OwnerController _controller;


        public OwnerControllerTest()
        {
            _mockOwnerService = new Mock<IOwnerService>();
            _mockValidator = new Mock<IValidator<CreateOwnerWithPokemonDto>>();
            _mockLogger = new Mock<ILogger<OwnerController>>();
            _controller = new OwnerController(_mockOwnerService.Object, _mockValidator.Object, _mockLogger.Object);
        }
        [Fact]
        public void CreateOwnerWithPokemon_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            // Act
            var result = _controller.CreateOwnerWithPokemon(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        [Fact]
        public void CreateOwnerWithPokemon_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var createDto = new CreateOwnerWithPokemonDto();
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Owner.FirstName", "First name is required."),
                new FluentValidation.Results.ValidationFailure("Pokemon.Name", "Pokemon name is required.")
            };

            _mockValidator.Setup(v => v.Validate(createDto))
                          .Returns(new FluentValidation.Results.ValidationResult(validationErrors));

            // Act
            var result = _controller.CreateOwnerWithPokemon(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            // Anonim türü kontrol etmek için
            var response = badRequestResult.Value;
            Assert.NotNull(response);

            var isValid = (bool)response.GetType().GetProperty("IsValid").GetValue(response);
            Assert.False(isValid);

            var errors = response.GetType().GetProperty("Errors").GetValue(response) as IEnumerable<object>;
            Assert.NotNull(errors);
            Assert.Equal(2, errors.Count());
        }



        [Fact]
        public void CreateOwnerWithPokemon_ShouldReturnInternalServerError_WhenServiceFails()
        {
            // Arrange
            var createDto = new CreateOwnerWithPokemonDto
            {
                Owner = new OwnerCreateDto { FirstName = "Ash", LastName = "Ketchum" },
                Pokemon = new PokemonCreateDto { Name = "Pikachu", BirthDate = System.DateTime.Now }
            };

            _mockValidator.Setup(v => v.Validate(createDto)).Returns(new FluentValidation.Results.ValidationResult());
            _mockOwnerService.Setup(s => s.CreateOwnerWithPokemon(createDto.Owner, createDto.Pokemon)).Returns(false);

            // Act
            var result = _controller.CreateOwnerWithPokemon(createDto);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverErrorResult.StatusCode);

            // Anonim türü kontrol etmek
            var value = serverErrorResult.Value;
            Assert.NotNull(value);
            Assert.False((bool)value.GetType().GetProperty("IsValid").GetValue(value));
            Assert.Equal("Something went wrong while saving the owner and pokemon.",
                         (string)value.GetType().GetProperty("Message").GetValue(value));
        }


        [Fact]
        public void CreateOwnerWithPokemon_ShouldReturnOk_WhenServiceSucceeds()
        {
            // Arrange
            var createDto = new CreateOwnerWithPokemonDto
            {
                Owner = new OwnerCreateDto { FirstName = "Ash", LastName = "Ketchum" },
                Pokemon = new PokemonCreateDto { Name = "Pikachu", BirthDate = System.DateTime.Now }
            };

            _mockValidator.Setup(v => v.Validate(createDto))
                          .Returns(new FluentValidation.Results.ValidationResult());
            _mockOwnerService.Setup(s => s.CreateOwnerWithPokemon(createDto.Owner, createDto.Pokemon))
                             .Returns(true);

            // Act
            var result = _controller.CreateOwnerWithPokemon(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Anonim türü kontrol etmek için
            var response = okResult.Value;
            Assert.NotNull(response);

            var isValid = (bool)response.GetType().GetProperty("IsValid").GetValue(response);
            Assert.True(isValid);

            var message = (string)response.GetType().GetProperty("Message").GetValue(response);
            Assert.Equal("Owner and Pokemon successfully created.", message);
        }


    }
    
}
