namespace Library_Management.Dtos
{
    public class BookResultDto
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public int PublishedYear { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string PublisherName { get; set; }
        public string? MemberFirstName { get; set; }
        public string? Comment { get; set; }
        public int? Rating { get; set; }
        public int? CopyNumber { get; set; }
        public string? LibraryName { get; set; }
    }
}
