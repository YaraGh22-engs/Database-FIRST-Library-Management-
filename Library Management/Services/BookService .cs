
using Library_Management.Dtos;
using Library_Management.Extension;
using Library_Management.IRepositories;
using Library_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Services
{
    public class BookService : IBookeService
    {
        private readonly IBookReo _bookRepo;  


        public BookService(IBookReo bookRepo)
        {
            _bookRepo = bookRepo; 
        }

        public async Task<bool> CreateBook(CreateUpdateBookDto dto)
        { 
            var book = new Book()
            {
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
                PublishedYear = dto.PublishedYear,
                PublisherId = dto.PublisherId,
                Quantity = dto.Quantity,
            };
            foreach (var libId in dto.LibrariesId)
            {
                book.BookLibraries.Add(new BookLibrary() { LibraryId = libId });
            }
            return await _bookRepo.Create(book);
        }

        public async Task<Bookdto> GetBookById(int id)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {id} not found");
            var bookdto = new Bookdto()
            {
                Title = book.Title,
                Author = book.Author.Name,
                Category = book.Category.Name,
                PublishedYear = book.PublishedYear,
                Publisher = book.Publisher.Name,
                Quantity = book.Quantity,
                //AvailableEditions = book.AvailableEditions,
                Libraries = book.BookLibraries.Select(bl => bl.Library.Name).ToList(),
                Reviews = book.Reviews.Select(r => r.Member.FirstName + ": " + r.Comment).ToList(),
                //Reviews = book.Reviews.Select(r => r.Comment).ToList(),
                Editions = book.BookEditions.Select(be => be.CopyNumber).ToList()

            };
            return bookdto;
        }

        public async Task<List<Bookdto>> GetBooks(string? searchKey)
        {
            var query = await _bookRepo.GetList().BookFilter(searchKey).ToListAsync();
            var books = query.Select(b => new Bookdto()
            {
                Title = b.Title,
                Author = b.Author.Name,
                Quantity = b.Quantity,
                //AvailableEditions = b.AvailableEditions,
                PublishedYear = b.PublishedYear,
                Publisher = b.Publisher.Name,
                Category = b.Category.Name,
                Libraries = b.BookLibraries.Select(bl => bl.Library.Name).ToList(),
                Reviews = b.Reviews.Select(r => r.Member.FirstName + ": " + r.Comment).ToList(),
                //Reviews = b.Reviews.Select(r => r.Comment).ToList(),
                Editions = b.BookEditions.Select(be => be.CopyNumber).ToList()
            });
            return books.ToList();
        }

        public async Task <bool> UpdateBook(CreateUpdateBookDto dto)
        {
            var book = await _bookRepo.GetById(dto.Id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {dto.Id} not found");
            book.Title = dto.Title;
            book.CategoryId = dto.CategoryId;
            book.AuthorId = dto.AuthorId;
            book.PublishedYear = dto.PublishedYear;
            book.PublisherId = dto.PublisherId;
            book.Quantity = dto.Quantity;
            // Update BookLibraries
            book.BookLibraries.Clear();
            foreach (var libId in dto.LibrariesId)
            {
                book.BookLibraries.Add(new BookLibrary() { LibraryId = libId });
            }
            return await _bookRepo.Update(book);
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _bookRepo.GetById(id);
            if (book == null)
                throw new KeyNotFoundException($"Book with id {id} not found");
            return  await _bookRepo.Remove(book);
        }


    }
}
