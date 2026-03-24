using System;
using System.Collections.Generic;

namespace Session2_2019.Models;

public partial class AssetGroup
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
