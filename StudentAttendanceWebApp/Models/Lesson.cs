using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public DateOnly LessonDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int ClassroomId { get; set; }

    public string LecturerId { get; set; } = null!;

    public string SubjectCode { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Classroom Classroom { get; set; } = null!;

    public virtual Lecturer Lecturer { get; set; } = null!;

    public virtual Subject SubjectCodeNavigation { get; set; } = null!;
}
