using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class Visit
{
    public long Id { get; set; }

    public long DoctorId { get; set; }

    public long PatientId { get; set; }

    public DateTime StartScheduledDate { get; set; }

    public DateTime EndScheduledDate { get; set; }

    public DateTime? StartActualDate { get; set; }

    public DateTime? EndActualDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int StatusId { get; set; }

    public virtual User Doctor { get; set; } = null!;

    public virtual User Patient { get; set; } = null!;

    public virtual VisitStatus Status { get; set; } = null!;

    public virtual ICollection<VisitsProcedure> VisitsProcedures { get; set; } = new List<VisitsProcedure>();
}
