using System.ComponentModel.DataAnnotations;

namespace Library_Management.Dtos
{
    public class CreateUpdateBookDto
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public int PublisherId { get; set; }

        [Range(1000, 2035, ErrorMessage = "Published year must be a valid year.")]
        public int? PublishedYear { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }
        public List<int> LibrariesId { get; set; } = new List<int>();
    }
}
