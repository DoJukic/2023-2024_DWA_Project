using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class BookLocationLink
{
    public int Idbllink { get; set; }

    public int LocationId { get; set; }

    public int BookId { get; set; }

    public int Total { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<UserBorrowingReservation> UserBorrowingReservations { get; set; } = new List<UserBorrowingReservation>();
}
