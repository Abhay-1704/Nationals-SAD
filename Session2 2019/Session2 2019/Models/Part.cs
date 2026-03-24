using System;
using System.Collections.Generic;

namespace Session2_2019.Models;

public partial class Part
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? EffectiveLife { get; set; }

    public virtual ICollection<ChangedPart> ChangedParts { get; set; } = new List<ChangedPart>();
}
