﻿using System;
using System.Collections.Generic;

namespace Clinic.Api.Models;

public partial class WeekDay
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<WeekDaySchedule> WeekDaySchedules { get; set; } = new List<WeekDaySchedule>();
}
