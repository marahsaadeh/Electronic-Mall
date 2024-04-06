using System;
using System.Collections.Generic;

namespace Electronic_Mall.Models;

public partial class User
{
    public int Userid { get; set; }

    public int Roleid { get; set; }

    public string Username { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual UserRole Role { get; set; } = null!;
}
