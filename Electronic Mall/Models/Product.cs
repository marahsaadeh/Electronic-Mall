using System;
using System.Collections.Generic;

namespace Electronic_Mall.Models;

public partial class Product
{
    public int Productid { get; set; }

    public int Categoryid { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    // public IFormFile? Photo { get; set; }
    public string? Photo { get; set; }
   
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;
}



