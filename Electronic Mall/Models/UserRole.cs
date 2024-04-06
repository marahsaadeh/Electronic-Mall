using System;
using System.Collections.Generic;

namespace Electronic_Mall.Models;

public partial class UserRole
{
    public int Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
