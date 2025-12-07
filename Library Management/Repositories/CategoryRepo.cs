using Library_Management.IRepositories;
using Library_Management.Models;

namespace Library_Management.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;

        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckExistence(int id)
        {
             var category = await _context.Categories.FindAsync(id);
             return category != null ? true : false;

        }
    }
}
