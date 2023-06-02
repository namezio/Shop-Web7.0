using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? IdOrder { get; set; }

    public int? IdProduct { get; set; }

    public decimal? SellingPrice { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public decimal? Amount { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
