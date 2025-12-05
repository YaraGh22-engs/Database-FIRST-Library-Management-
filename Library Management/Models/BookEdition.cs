using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class BookEdition
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int CopyNumber { get; set; }

    public string? Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
