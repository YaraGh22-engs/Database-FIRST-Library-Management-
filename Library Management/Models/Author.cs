using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Biography { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
