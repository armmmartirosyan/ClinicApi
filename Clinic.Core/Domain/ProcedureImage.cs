using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class ProcedureImage
{
    public long Id { get; set; }

    public string Url { get; set; } = null!;

    public long VisitProcedureId { get; set; }

    public virtual VisitsProcedure VisitProcedure { get; set; } = null!;
}
