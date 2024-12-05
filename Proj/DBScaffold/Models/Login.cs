using System;
using System.Collections.Generic;

namespace DBScaffold.Models;

public partial class Login
{
    public int Idlogin { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
