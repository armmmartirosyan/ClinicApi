using System;
using System.Collections.Generic;

namespace Clinic.Api.Models;

public partial class Procedure
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<VisitsProcedure> VisitsProcedures { get; set; } = new List<VisitsProcedure>();
}
