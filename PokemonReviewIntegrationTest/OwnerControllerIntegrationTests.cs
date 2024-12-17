using PokemonReviewApp.Dtos.OwnerDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PokemonReviewApp.Data;

namespace PokemonReviewIntegrationTest
{
    public class OwnerControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OwnerControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetOwners_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/Owner");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task CreateOwnerWithPokemon_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var requestDto = new CreateOwnerWithPokemonDto
            {
                Owner = new OwnerCreateDto
                {
                    FirstName = "Ash45",
                    LastName = "Ketchum",
                    Gym = "Pallet Town",
                    CountryId = 1
                },
                Pokemon = new PokemonCreateDto
                {
                    Name = "Pikachu45",
                    BirthDate = DateTime.Now,
                    CategoryId = 1  // Yeni eklenen CategoryId alanı
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Owner", requestDto);

            // Assert
            response.EnsureSuccessStatusCode(); // 200 OK

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
           
            //var responseContent = await response.Content.ReadFromJsonAsync<dynamic>();
            //Assert.True(responseContent.IsValid);
            //Assert.Equal("Owner and Pokemon successfully created.", responseContent.Message);
        }

    }
}
