using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int EditionId { get; set; }

    public int MemberId { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual BookEdition Edition { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Member Member { get; set; } = null!;
}
