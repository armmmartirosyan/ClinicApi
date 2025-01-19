using System;
using System.Collections.Generic;
using System.Numerics;

namespace Clinic.Core.Domain;

public partial class DoctorsSpecialization
{
    public required long DoctorId { get; set; }

    public required int SpecializationId { get; set; }

    public virtual User Doctor { get; set; }

    public virtual Specialization Specialization { get; set; }
}
