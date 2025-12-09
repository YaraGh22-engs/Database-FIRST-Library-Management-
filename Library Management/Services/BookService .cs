
using Library_Management.Dtos;
using Library_Management.Extension;
using Library_Management.IRepositories;
using Library_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Library_Management.Services
{
    public class BookService : IBookeService
    {
        private readonly IBookReo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IPublisherRepo _publisherRepo;
        private readonly ILIbraryRepo _IbraryRepo; 

        public BookService(IBookReo bookRepo,
                            IAuthorRepo authorRepo,
                            ICategoryRepo categoryRepo,
                            IPublisherRepo publisherRepo,
                            ILIbraryRepo libraryRepo 
                            )
        {
            _bookRepo = bookRepo; 
            _authorRepo = authorRepo; 
            _categoryRepo = categoryRepo;
            _publisherRepo = publisherRepo;
            _IbraryRepo = libraryRepo; 

        }

        public async Task<bool> CreateBook(CreateUpdateBookDto dto)
        {
            // Add a new book and verify Author ID, Category ID, Publisher ID and Libraries  Before saving, ensure the correct link is established.
            
            // Validation
            var autor = await _authorRepo.CheckExistence(dto.AuthorId);
            if (!autor)
                throw new ArgumentException("Author is not existed", nameof(autor));

            var category = await _categoryRepo.CheckExistence(dto.CategoryId);
            if (!category)
                throw new ArgumentException("Category is not existed", nameof(category));

            var publisher = await _publisherRepo.CheckExistence(dto.PublisherId);
            if (!publisher)
                throw new ArgumentException("Publisher is not existed", nameof(publisher));

            foreach (var libId in dto.LibrariesId)
            {
                var library = await _IbraryRepo.CheckExistence(libId);
                if (!library)
                    throw new ArgumentException($"Library with the id {libId} is not existed", nameof(library));
            }
            // dto to entity
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

        // GetBooks2 :ADO + SP
        public async Task<List<Bookdto>> GetBooksSql(string searchKey)
        {
            return await _bookRepo.GetBooksSql(searchKey);
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

        //ADO + SP 
        public async Task<List<BookRatingDto>> GetBooksRate()
        {
            return await _bookRepo.GetBooksRating();
        }
        //Linq 
        public async Task<List<BookRatingDto>> GetBooksRate2()
        {
            return   _bookRepo.GetBooksRating2()
                              .AsEnumerable()  // ✅ الحل: Client Evaluation //  // ✅ تنفيذ SQL + تحميل البيانات للذاكرة
                              .GroupBy(b => new { b.Title, b.Publisher.Name }) // Grouping in memory
                              .Select(g => new BookRatingDto // Projection in memory
                              {
                                  Title = g.Key.Title, // Accessing grouped key
                                  PublisherName = g.Key.Name ?? "",
                                  AverageRating = g.Average(b => b.Reviews.Average(r => (double)r.Rating))
                              })
                              .OrderByDescending(x => x.AverageRating) //
                              .ToList();
        }

         

    }
}
