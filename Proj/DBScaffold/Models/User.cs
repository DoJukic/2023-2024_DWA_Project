using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class User
{
    public int Iduser { get; set; }

    public string Name { get; set; } = null!;

    public string? MiddleNames { get; set; }

    public string Surname { get; set; } = null!;

    public int LoginId { get; set; }

    public virtual Administrator? Administrator { get; set; }

    public virtual Login Login { get; set; } = null!;

    public virtual ICollection<UserBorrowingReservation> UserBorrowingReservations { get; set; } = new List<UserBorrowingReservation>();
}
