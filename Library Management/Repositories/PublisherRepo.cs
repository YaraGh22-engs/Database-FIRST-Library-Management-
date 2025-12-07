using Library_Management.IRepositories;
using Library_Management.Models;

namespace Library_Management.Repositories
{
    public class PublisherRepo : IPublisherRepo
    {
        private AppDbContext _context;
        public PublisherRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckExistence(int id)
        { 
            var publisher = await _context.Publishers.FindAsync(id);
            return publisher != null ? true : false ;
        }
    }
}
