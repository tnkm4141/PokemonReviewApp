using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DataContext _context;
    
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {

           _context.Add(category);
            return save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();  
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _context.PokemonCategories.Where(p=>p.CategoryId == categoryId).Select(p=>p.Pokemon).ToList();
        }
        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return save();
        }

        public bool save()
        {
            var saved = _context.SaveChanges();
            return saved>0?true:false;
        }
    }
}
