using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class Entity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Scoring> Scorings { get; set; } = new List<Scoring>();
}
