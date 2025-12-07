using Library_Management.IRepositories;
using Library_Management.Models;

namespace Library_Management.Repositories
{
    public class LibraryRepo : ILIbraryRepo
    {
        private AppDbContext _context;
        public LibraryRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckExistence(int id)
        {
            var library = await _context.Libraries.FindAsync(id);
            return library != null ? true : false;
        }
    }
}
