using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models;

public partial class Book
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public int Length { get; set; }
    [Required]
    public int Width { get; set; }
    [Required]
    public int Pages { get; set; }

    [Required(ErrorMessage = "Nhập giá sản phẩm")]
    public double UnitPrice { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public bool Available { get; set; } = false;

    public int CategoryId { get; set; }

    public string? SupplierId { get; set; }

    [Required(ErrorMessage = "Nhập số lượng sản phẩm")]
    public int Quantity { get; set; }

    public double Discount { get; set; }

    public bool Special { get; set; } = false;

    public bool Latest { get; set; } = false;

    [NotMapped]
    public IFormFile bookFile { get; set; }

    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}
