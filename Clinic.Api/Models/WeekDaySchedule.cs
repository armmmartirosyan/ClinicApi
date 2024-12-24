using System;
using System.Collections.Generic;

namespace Clinic.Api.Models;

public partial class WeekDaySchedule
{
    public long Id { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public TimeOnly BreakStartTime { get; set; }

    public TimeOnly BreakEndTime { get; set; }

    public long DoctorId { get; set; }

    public int WeekDayId { get; set; }

    public virtual User Doctor { get; set; } = null!;

    public virtual WeekDay WeekDay { get; set; } = null!;
}
