using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Location
{
    public int Idlocation { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<BookLocationLink> BookLocationLinks { get; set; } = new List<BookLocationLink>();
}
