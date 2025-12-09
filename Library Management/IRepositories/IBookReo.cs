using Library_Management.Dtos;
using Library_Management.Models;

namespace Library_Management.IRepositories
{
    public interface IBookReo
    {
        //LINQ
        IQueryable<Book> GetList();
        //SP 
        Task<List<Bookdto>> GetBooksSql(string? searchKey); 
        Task<Book> GetById(int id);
        Task<bool> Create(Book book);
        Task<bool> Update(Book book);
        Task<bool> Remove(Book book);
        //ADO + SP 
        Task<List<BookRatingDto>> GetBooksRating(); //  قائمة كتب مع متوسط التقييمات لكل كتاب
        //LINQ
        IQueryable<Book> GetBooksRating2();

        
    }
}
