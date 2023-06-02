using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class Order
{
    public int Id { get; set; }

    public string? OrderName { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? DoneDate { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }

    public int? IdVocher { get; set; }

    public int? IdUser { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
