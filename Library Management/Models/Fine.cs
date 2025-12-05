using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Fine
{
    public int FineId { get; set; }

    public int TransactionId { get; set; }

    public decimal Amount { get; set; }

    public bool? Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
}
