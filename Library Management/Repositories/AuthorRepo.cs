using Library_Management.IRepositories;
using Library_Management.Models;

namespace Library_Management.Repositories
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext _context;
        public AuthorRepo(AppDbContext context) {
            _context=context;
        }
        public async Task<bool> CheckExistence(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author != null ? true : false;
        }
    }
}
