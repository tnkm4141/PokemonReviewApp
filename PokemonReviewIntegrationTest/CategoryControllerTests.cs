using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PokemonReviewApp.Dtos.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PokemonReviewIntegrationTest
{
    public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/categories");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnOk_WhenValidData()
        {
            // Arrange
            var category = new CategoryCreateDto { Name = "Test Category" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/categories", category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
