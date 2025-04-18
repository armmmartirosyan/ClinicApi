﻿using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class NotWorkingDay
{
    public long Id { get; set; }

    public DateOnly NotWorkDate { get; set; }

    public long DoctorId { get; set; }

    public virtual User Doctor { get; set; } = null!;
}
