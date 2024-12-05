using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace DBScaffold.Models;

public partial class Administrator
{
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
