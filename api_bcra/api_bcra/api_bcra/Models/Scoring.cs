using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class Scoring
{
    public int Id { get; set; }

    public string Period { get; set; } = null!;

    public int Situation { get; set; }

    public int Amount { get; set; }

    public bool Checking { get; set; }

    public bool ProcJu { get; set; }

    public int? Entity { get; set; }

    public virtual Entity? EntityNavigation { get; set; }
}
