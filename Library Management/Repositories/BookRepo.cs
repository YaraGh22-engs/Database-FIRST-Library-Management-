using Library_Management.IRepositories;
using Library_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Repositories
{
    public class BookRepo : IBookReo
    {
        private readonly AppDbContext _context;

        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Book book)
        {
            await _context.Books.AddAsync(book);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.Member)
                .Include(b => b.BookEditions)
                .Include(b => b.BookLibraries)
                .ThenInclude(bl => bl.Library)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public IQueryable<Book> GetList()
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.Member)
                .Include(b => b.BookEditions)
                .Include(b => b.BookLibraries)
                .ThenInclude(bl => bl.Library)
                .AsNoTracking();
            return query;
        }

        public async Task<bool> Remove(Book book)
        {
            _context.Books.Remove(book);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> Update(Book book)
        {
            _context.Books.Update(book);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
