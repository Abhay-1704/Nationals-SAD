using System;
using System.Collections.Generic;

namespace Session2_2019.Models;

public partial class DepartmentLocation
{
    public long Id { get; set; }

    public long DepartmentId { get; set; }

    public long LocationId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual Department Department { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
