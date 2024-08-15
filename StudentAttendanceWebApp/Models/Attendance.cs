using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string StudentId { get; set; } = null!;

    public int? DurationMinutes { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
