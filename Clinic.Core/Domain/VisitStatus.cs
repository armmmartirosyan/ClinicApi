using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class VisitStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
