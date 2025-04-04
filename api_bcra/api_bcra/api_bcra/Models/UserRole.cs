using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? IdRole { get; set; }

    public int? IdUser { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
