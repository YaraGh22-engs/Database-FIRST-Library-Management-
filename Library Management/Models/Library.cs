using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Library
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public virtual ICollection<BookLibrary> BookLibraries { get; set; } = new List<BookLibrary>();
}
