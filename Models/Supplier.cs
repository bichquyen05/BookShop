using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public partial class Supplier
{
    [Key]
    [Required(ErrorMessage = "Id is required!")]
    public string Id { get; set; }

    [Required(ErrorMessage = "Name is required!")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone is required!")]
    public string Phone { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
