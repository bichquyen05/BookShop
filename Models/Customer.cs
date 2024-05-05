using System;
using System.Collections.Generic;

namespace BookShop.Models;

public partial class Customer
{
    public string Id { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool Activated { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
