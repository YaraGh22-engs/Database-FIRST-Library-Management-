using Library_Management.Dtos;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Library_Management.Services
{
    public interface IBookeService
    {
        Task<List<Bookdto>> GetBooks(string? searchKey);
        Task<Bookdto> GetBookById(int id);
        Task<bool> CreateBook(CreateUpdateBookDto dto);
        Task<bool> UpdateBook( CreateUpdateBookDto dto);
        Task<bool> DeleteBook(int id);
        //for ADO + SP 
        Task<List<BookRatingDto>> GetBooksRate();

        //for LINQ
        Task<List<BookRatingDto>> GetBooksRate2();
    }
}
