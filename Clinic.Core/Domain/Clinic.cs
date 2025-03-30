using System;
using System.Collections.Generic;

namespace Clinic.Core.Domain;

public partial class Clinic
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
