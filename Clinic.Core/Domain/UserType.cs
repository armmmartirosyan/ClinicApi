﻿using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class UserType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
