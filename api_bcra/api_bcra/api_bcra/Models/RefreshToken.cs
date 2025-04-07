using System;
using System.Collections.Generic;

namespace api_bcra.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public int? IdUser { get; set; }

    public byte Used { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
