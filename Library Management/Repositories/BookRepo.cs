using Humanizer;
using Library_Management.Dtos;
using Library_Management.IRepositories;
using Library_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Net;

namespace Library_Management.Repositories
{
    public class BookRepo : IBookReo
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public BookRepo(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ConnectionStrings");
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
        //LINQ
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

        // GetList2 SP + ADO.NET  
        public async Task<List<Bookdto>> GetBooksSql(string? searchKey)
        {
            var result = new List<Bookdto>(); // Final aggregated result list 

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetBooksList";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "@searchKey";
                    param.Value = string.IsNullOrEmpty(searchKey) ? "" : searchKey;
                    command.Parameters.Add(param);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Cache ordinals for performance
                        var ordTitle = reader.GetOrdinal("book_title");
                        var ordAuthor = reader.GetOrdinal("author_name");
                        var ordCategory = reader.GetOrdinal("category_name");
                        var ordPublisher = reader.GetOrdinal("publisher_name");
                        var ordPublishedYear = reader.GetOrdinal("PublishedYear");
                        var ordQuantity = reader.GetOrdinal("Quantity");
                        var ordCopy = reader.GetOrdinal("book_copy");
                        var ordLibrary = reader.GetOrdinal("library_name");
                        var ordComment = reader.GetOrdinal("Comment");
                        var ordMemberFirst = reader.GetOrdinal("MemberFirstName");

                        while (await reader.ReadAsync())
                        {
                            // Basic book info 
                            string title =  reader.GetString(ordTitle);
                            string author = reader.GetString(ordAuthor);
                            string category = reader.GetString(ordCategory);
                            string publisher = reader.GetString(ordPublisher);
                            int? publishedYear =  reader.GetInt32(ordPublishedYear) ;
                            int quantity = reader.GetInt32(ordQuantity);

                            // Find or create aggregated DTO for this title
                            var book = result.FirstOrDefault(b => b.Title == title); // Assuming title is unique identifier
                            if (book == null)   // New book entry
                            {
                                book = new Bookdto
                                {
                                    Title = title,
                                    Author = author,
                                    Category = category,
                                    Publisher = publisher,
                                    PublishedYear = publishedYear,
                                    Quantity = quantity,
                                    Libraries = new List<string>(),
                                    Reviews = new List<string>(),
                                    Editions = new List<int>()
                                };
                                result.Add(book);
                            }

                            // Book edition 
                            var copyNumber = reader.GetInt32(ordCopy);
                                if (!book.Editions.Contains(copyNumber))
                                // Add if not already present 
                                book.Editions.Add(copyNumber);


                            // Library name 
                            var libraryName = reader.GetString(ordLibrary); 
                                if (!book.Libraries.Contains(libraryName))
                                book.Libraries.Add(libraryName);


                            // Review comment + member name combined
                            string comment = reader.GetString(ordComment);
                            string memberFirst = reader.GetString(ordMemberFirst);

                            string combinedReview = string.Empty;
                                   combinedReview = $"{memberFirst}: {comment}"; 
                            if ( !book.Reviews.Contains(combinedReview))
                                  book.Reviews.Add(combinedReview);
                        }
                    }
                }
            }

            return result;
        }
        //StoredProcedure in SSMS ====
        //============================
        //create procedure GetBooksList
        //@searchKey nvarchar(100)
        //as
        //begin
        //SELECT
        //b.Id,
        //b.Title as book_title,
        //be.CopyNumber as book_copy , 
        //au.Name as author_name,
        //cat.Name as category_name ,
        //pub.Name as publisher_name,
        //m.FirstName AS MemberFirstName,
        //b.PublishedYear , 
        //b.Quantity, 
        //lb.Name as library_name,
        //rev.Comment
        //FROM book b
        //JOIN Category cat ON b.CategoryId = cat.Id
        //JOIN Author au ON au.Id = b.AuthorId
        //JOIN Publisher pub ON pub.Id = b.PublisherId
        //LEFT JOIN Review rev ON rev.BookId = b.Id
        //LEFT JOIN Member m ON rev.MemberId = m.Id
        //LEFT JOIN BookLibrary bl ON b.Id = bl.BookId
        //LEFT JOIN Library lb ON lb.Id = bl.LibraryId
        //LEFT join BookEdition be on be.BookId = b.Id

        //where
        //b.Title like '%' +@searchKey +'%' OR
        //au.Name like '%' + @SearchKey + '%' OR
        //cat.Name like '%' + @SearchKey + '%' OR
        //pub.Name like '%' + @SearchKey + '%' OR
        //lb.Name like '%' + @searchKey + '%' ;
        //        end


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

        //Raw ADO.NET + Stored Procedure
        public async Task<List<BookRatingDto>> GetBooksRating()
        {
            // 1. إنشاء قائمة فارغة للنتائج
            var result = new List<BookRatingDto>();
            // 2. الحصول على Connection من EF Core
            using (var connection = _context.Database.GetDbConnection())
            {
                // 3. فتح الاتصال بقاعدة البيانات
                await connection.OpenAsync();
                // 4. إنشاء أمر تنفيذ Stored Procedure
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "GetBookAverageRatings"; // اسم الـ SP
                    command.CommandType = System.Data.CommandType.StoredProcedure; // تحديد نوع الأمر كـ Stored Procedure
                    // 5. تنفيذ الأمر وقراءة النتائج
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // 6. قراءة كل صف من النتائج
                        while (await reader.ReadAsync())
                        {
                            // 7.  تحويل كل صف إلى DTO
                            var bookRating = new BookRatingDto()
                            { 
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                PublisherName = reader.GetString(reader.GetOrdinal("publisherName")),
                                AverageRating = reader.GetInt32(reader.GetOrdinal("AverageRating"))
                            };
                            result.Add(bookRating);
                        }
                    }
                }
            }
            return result;
        }


        //STORED PROCEDURE IN SSMS
//       CREATE PROCEDURE GetBookAverageRatings
//        AS
//        BEGIN
//        SELECT
//        b.Title,
//        p.Name AS publisherName,
//        AVG(r.Rating) AS AverageRating
//        FROM Book b
//        JOIN Publisher p ON b.PublisherId = p.Id
//        LEFT JOIN Review r ON b.Id = r.BookId
//        GROUP BY b.Id, b.Title, p.Name
//        END
        
        public IQueryable <Book> GetBooksRating2()
        {
            return _context.Books.Include(p => p.Publisher)
                                        .Include(r => r.Reviews)
                                        .AsNoTracking();
             
        }

    }
}
