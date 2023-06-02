using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public decimal? SellingPrice { get; set; }

    public string? CorverImageUrl { get; set; }

    public string? Description { get; set; }

    public int? Qty { get; set; }

    public int? IdCategory { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsBlocked { get; set; }

    public virtual ProductCategory? IdCategoryNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
