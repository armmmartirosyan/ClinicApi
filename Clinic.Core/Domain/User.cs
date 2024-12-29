using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class User
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Password { get; set; }

    public DateOnly BirthDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int TypesId { get; set; }

    public virtual ICollection<MedicinesAssigned> MedicinesAssignedDoctors { get; set; } = new List<MedicinesAssigned>();

    public virtual ICollection<MedicinesAssigned> MedicinesAssignedPatients { get; set; } = new List<MedicinesAssigned>();

    public virtual ICollection<NotWorkingDay> NotWorkingDays { get; set; } = new List<NotWorkingDay>();

    public virtual UserType Types { get; set; } = null!;

    public virtual ICollection<Visit> VisitDoctors { get; set; } = new List<Visit>();

    public virtual ICollection<Visit> VisitPatients { get; set; } = new List<Visit>();

    public virtual ICollection<WeekDaySchedule> WeekDaySchedules { get; set; } = new List<WeekDaySchedule>();

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
