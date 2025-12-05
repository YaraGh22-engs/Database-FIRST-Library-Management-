using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int BookId { get; set; }

    public int MemberId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
