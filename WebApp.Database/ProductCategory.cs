using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ThumbnailUrl { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
