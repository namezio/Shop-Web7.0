using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class UserType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsOwner { get; set; }

    public bool? IsBackUser { get; set; }

    public bool? IsEndUser { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
