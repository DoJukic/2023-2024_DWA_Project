using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Genre
{
    public int Idgenre { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
