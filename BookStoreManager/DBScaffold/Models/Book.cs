using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Book
{
    public int Idbook { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? GenreId { get; set; }

    public virtual ICollection<BookLocationLink> BookLocationLinks { get; set; } = new List<BookLocationLink>();

    public virtual Genre? Genre { get; set; }
}
