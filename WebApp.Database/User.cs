using System;
using System.Collections.Generic;

namespace WebApp.Database;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string AvartarUrl { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public int IdUserType { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool IsBlocked { get; set; }

    public bool IsDeleted { get; set; }

    public virtual UserType? IdUserTypeNavigation { get; set; }
}
