using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Administrator
{
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
