using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class Query
{
    public int Id { get; set; }

    public string? CUIT { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateQuery { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
