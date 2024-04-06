using System;
using System.Collections.Generic;

namespace Electronic_Mall.Models;

public partial class Cart
{
    public int Cartid { get; set; }

    public int Userid { get; set; }

    public int Productid { get; set; }

    public int? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
