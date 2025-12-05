using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
