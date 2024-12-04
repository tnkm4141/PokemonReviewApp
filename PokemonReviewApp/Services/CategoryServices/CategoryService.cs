using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dtos.CategoryDtos;
using PokemonReviewApp.Dtos.PokemonDtos;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
       // private DataContext _context;
        private readonly IMapper _mapper;

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public ICollection<CategoryDto> GetAllCategories()
        {
            var categories = _categoryRepository.GetCategories();

            // Category'den CategoryDto'ya dönüşüm
            return _mapper.Map<ICollection<CategoryDto>>(categories);
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            // Category'den CategoryDto'ya dönüşüm
            return _mapper.Map<CategoryDto>(category);
        }


        public ICollection<PokemonDto> GetPokemonsByCategory(int categoryId)
        {
            var pokemons = _categoryRepository.GetPokemonByCategory(categoryId);

            // Pokemon'dan PokemonDto'ya dönüşüm
            return _mapper.Map<ICollection<PokemonDto>>(pokemons);
        }

        public bool CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            // CategoryDto'dan Category'ye dönüşüm
            var category = _mapper.Map<Category>(categoryCreateDto);

            if (category == null)
            {
                return false;
            }

            return _categoryRepository.CreateCategory(category);
        }

        public bool UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            // CategoryDto'dan Category'ye dönüşüm
            var category = _mapper.Map<Category>(categoryUpdateDto);

            if (category == null)
            {
                return false;
            }

            return _categoryRepository.UpdateCategory(category);
        }

        public bool DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return false;
            }

            return _categoryRepository.DeleteCategory(category);
        }

        public bool CategoryExists(int id)
        {
            return _categoryRepository.CategoryExists(id);
        }
    }
}
