using System;
using System.Collections.Generic;

namespace Library_Management.Models;

public partial class Member
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? MembershipDate { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
