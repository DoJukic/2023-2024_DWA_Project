using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class UserBorrowingReservation
{
    public int Idreservation { get; set; }

    public int BllinkId { get; set; }

    public int UserId { get; set; }

    public DateTimeOffset DateReserved { get; set; }

    public DateTimeOffset DateExpiration { get; set; }

    public DateTimeOffset? DateBorrowed { get; set; }

    public DateTimeOffset? DateReturned { get; set; }

    public virtual BookLocationLink Bllink { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
