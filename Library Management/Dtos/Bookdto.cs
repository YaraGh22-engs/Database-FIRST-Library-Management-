

using Library_Management.Models;

namespace Library_Management.Dtos
{
    public class Bookdto
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; }

        public string Category { get; set; }

        public string Publisher { get; set; }

        public int? PublishedYear { get; set; }

        public int Quantity { get; set; }

        //public int? AvailableEditions { get; set; }
        public List<string> Libraries { get; set; } = new List<string>();
        public List<string> Reviews { get; set; } = new List<string>();
        public List<int> Editions { get; set; } = new List<int>();

         
    }
}
