using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Login
{
    public int Idlogin { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordPlain { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
