using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Order
{
    public int Id { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime RequireDate { get; set; }

    public string Receiver { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public double Amount { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
