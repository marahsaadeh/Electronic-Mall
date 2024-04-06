using System;
using System.Collections.Generic;

namespace Electronic_Mall.Models;

public partial class Category
{
    public Category() {
        Products=new HashSet<Product>();
    }
    public int Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public string? Photo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
