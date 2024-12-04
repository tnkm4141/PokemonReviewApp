using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.CategoryServices
{
    public interface ICategoryService
    {
        ICollection<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        ICollection<PokemonDto> GetPokemonsByCategory(int categoryId);
        bool CreateCategory(CategoryCreateDto categoryCreateDto);
        bool UpdateCategory(CategoryUpdateDto categoryUpdateDto);
        bool DeleteCategory(int id);  // Silme işlemi için ID yeterli olabilir.
        bool CategoryExists(int id);
    }
}
