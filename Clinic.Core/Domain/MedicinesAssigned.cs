using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class MedicinesAssigned
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Dose { get; set; } = null!;

    public string? Notes { get; set; }

    public DateOnly StartDate { get; set; }

    public int Quantity { get; set; }

    public int DayCount { get; set; }

    public long DoctorId { get; set; }

    public long PatientId { get; set; }

    public long VisitProcedureId { get; set; }

    public virtual User Doctor { get; set; } = null!;

    public virtual User Patient { get; set; } = null!;

    public virtual VisitsProcedure VisitProcedure { get; set; } = null!;
}
