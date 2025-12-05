using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class BookLibrary
{
    public int BookId { get; set; }

    public int LibraryId { get; set; }

    public string? AdditionalDetails { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Library Library { get; set; } = null!;
}
