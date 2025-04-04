using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Query> Queries { get; set; } = new List<Query>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
