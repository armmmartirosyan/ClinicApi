using System;
using System.Collections.Generic;

namespace Clinic.Api.Models;

public partial class VisitsProcedure
{
    public long Id { get; set; }

    public long VisitId { get; set; }

    public long ProcedureId { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<MedicinesAssigned> MedicinesAssigneds { get; set; } = new List<MedicinesAssigned>();

    public virtual Procedure Procedure { get; set; } = null!;

    public virtual ICollection<ProcedureImage> ProcedureImages { get; set; } = new List<ProcedureImage>();

    public virtual Visit Visit { get; set; } = null!;
}
