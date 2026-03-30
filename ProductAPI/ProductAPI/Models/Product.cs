using System;
using System.Collections.Generic;

namespace ProductAPI.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }

    public int CategoryId { get; set; }

    public virtual Category? Category { get; set; } = null!;
}
