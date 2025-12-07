using Library_Management.Dtos;
using Library_Management.Models;

namespace Library_Management.IRepositories
{
    public interface IBookReo
    {
        IQueryable<Book> GetList();
        Task<Book> GetById(int id);
        Task<bool> Create(Book book);
        Task<bool> Update(Book book);
        Task<bool> Remove(Book book);
        Task<List<BookRatingDto>> GetBooksRating(); //  قائمة كتب مع متوسط التقييمات لكل كتاب
    }
}
