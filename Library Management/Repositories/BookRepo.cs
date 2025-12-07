using Humanizer;
using Library_Management.Dtos;
using Library_Management.IRepositories;
using Library_Management.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

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


    }
}
