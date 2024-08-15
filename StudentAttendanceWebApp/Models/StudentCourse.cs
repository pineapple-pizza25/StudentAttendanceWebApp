using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class StudentCourse
{
    public int Id { get; set; }

    public string StudentId { get; set; } = null!;

    public string ModuleCode { get; set; } = null!;

    public virtual Subject ModuleCodeNavigation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
