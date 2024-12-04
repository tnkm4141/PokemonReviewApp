using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.CategoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Interfaces;
using Xunit;
using PokemonReviewApp.Dtos.PokemonDtos;

namespace PokemonReviewTest
{
  public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _controller;
        private readonly List<CategoryDto> categories;

        public CategoryControllerTest()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller= new CategoryController(_mockCategoryService.Object);
            categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Electric" },
                new CategoryDto { Id = 2, Name = "Water" }
            };
        }

        [Fact]
        public void GetCategories_ActionExecutes_ReturnOkWithCategories()
        {
            //arrange
            _mockCategoryService.Setup(s => s.GetAllCategories()).Returns(categories);
           
            // act
            var result = _controller.GetCategories();
          
            //Assert
            var OkResult=Assert.IsType<OkObjectResult>(result);
            var resturnCategories= Assert.IsAssignableFrom<List<CategoryDto>>(OkResult.Value);
            Assert.Equal<int>(2, resturnCategories.ToList().Count);
        }

        [Fact]
        public void GetCategory_IdInValid_ReturnNotFound()
        {
            
            _mockCategoryService.Setup(s => s.GetCategoryById(1)).Returns((CategoryDto)null);

            
            var result = _controller.GetCategory(1);

            
            Assert.IsType<NotFoundResult>(result);

        }
        [Theory, InlineData(1), InlineData(2)]
        public void GetCategory_IdIValid_ReturnOkResult(int categoryId)
        {
            var category = categories.First(x=>x.Id==categoryId);
            _mockCategoryService.Setup(s => s.GetCategoryById(categoryId)).Returns(category);

          
            var result = _controller.GetCategory(categoryId);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
             Assert.Equal(category, okResult.Value);
            var returnCategory= Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal(categoryId, returnCategory.Id);
            Assert.Equal(category.Name, returnCategory.Name);
            

        }

        [Fact]
        public void GetPokemonByCategoryId_ReturnsNotFound_WhenNoPokemonsExist()
        {
            
            _mockCategoryService.Setup(s => s.GetPokemonsByCategory(1)).Returns(new List<PokemonDto>());

            
            var result = _controller.GetPokemonByCategoryId(1);

           
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public void GetPokemonByCategoryId_ReturnsOkResult_WhenPokemonsExist()
        {
            var pokemons = new List<PokemonDto>
            {
                new PokemonDto { Id = 1, Name = "Pikachu", BirthDate = new DateTime(1997, 1, 1) },
                new PokemonDto { Id = 2, Name = "Squirtle", BirthDate = new DateTime(1999, 5, 15) }
            };
            _mockCategoryService.Setup(s => s.GetPokemonsByCategory(1)).Returns(pokemons);

            
            var result = _controller.GetPokemonByCategoryId(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(pokemons, okResult.Value);
        }
        [Fact]
        public void CreateCategory_ReturnsBadRequest_WhenCategoryDtoIsNull()
        {
            
            var result = _controller.CreateCategory(null);

            
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void CreateCategory_ReturnsOkResult_WhenCategoryIsCreated()
        {
            
            var categoryDto = new CategoryCreateDto { Name = "Electric" };
            _mockCategoryService.Setup(s => s.CreateCategory(categoryDto)).Returns(true);

            
            var result = _controller.CreateCategory(categoryDto);

            
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void CreateCategory_ReturnsInternalServerError_WhenServiceFailsToCreateCategory()
        {
          
            var categoryDto = new CategoryCreateDto { Name = "Electric" };
            _mockCategoryService.Setup(s => s.CreateCategory(categoryDto)).Returns(false);

            
            var result = _controller.CreateCategory(categoryDto);

         
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error creating category", statusCodeResult.Value);
        }

        [Fact]
        public void UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
           
            var categoryDto = new CategoryUpdateDto { Id = 1, Name = "Updated Name" };
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(false);

         
            var result = _controller.UpdateCategory(1, categoryDto);

        
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void UpdateCategory_ReturnsOkResult_WhenCategoryIsUpdated()
        {
            // Arrange
            var categoryDto = new CategoryUpdateDto { Id = 1, Name = "Updated Name" };
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(true);
            _mockCategoryService.Setup(s => s.UpdateCategory(categoryDto)).Returns(true);

            // Act
            var result = _controller.UpdateCategory(1, categoryDto);

            // Assert
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void UpdateCategory_ReturnsBadRequest_WhenCategoryDtoIsNull()
        {
           
            var result = _controller.UpdateCategory(1, null);

            
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void UpdateCategory_ReturnsBadRequest_WhenIdDoesNotMatchCategoryDtoId()
        {
           
            var categoryDto = new CategoryUpdateDto { Id = 2, Name = "Invalid Id" }; // Id uyuşmuyor

            
            var result = _controller.UpdateCategory(1, categoryDto);

           
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateCategory_ReturnsInternalServerError_WhenServiceFailsToUpdateCategory()
        {
           
            var categoryDto = new CategoryUpdateDto { Id = 1, Name = "Updated Name" };
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(true); // Kategori mevcut
            _mockCategoryService.Setup(s => s.UpdateCategory(categoryDto)).Returns(false); // Güncelleme başarısız

           
            var result = _controller.UpdateCategory(1, categoryDto);

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error updating category", statusCodeResult.Value);
        }

        [Fact]
        public void DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
           
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(false);

            
            var result = _controller.DeleteCategory(1);

            
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void DeleteCategory_ReturnsOkResult_WhenCategoryIsDeleted()
        {
            
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(true);
            _mockCategoryService.Setup(s => s.DeleteCategory(1)).Returns(true);

            
            var result = _controller.DeleteCategory(1);

            
            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public void DeleteCategory_ReturnsInternalServerError_WhenServiceFailsToDeleteCategory()
        {
          
            _mockCategoryService.Setup(s => s.CategoryExists(1)).Returns(true); // Kategori mevcut
            _mockCategoryService.Setup(s => s.DeleteCategory(1)).Returns(false); // Silme işlemi başarısız

            
            var result = _controller.DeleteCategory(1);

            
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error deleting category", statusCodeResult.Value);
        }


    }
}
