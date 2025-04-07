using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class Person
{
    public int Id { get; set; }
    public string Cuit { get; set; } = null!;
    public string Fullname { get; set; } = null!;
    public virtual ICollection<Scoring> Scorings { get; set; } = new List<Scoring>();
}
