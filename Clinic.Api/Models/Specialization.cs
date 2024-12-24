using System;
using System.Collections.Generic;

namespace Clinic.Api.Models;

public partial class Specialization
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Doctors { get; set; } = new List<User>();
}
