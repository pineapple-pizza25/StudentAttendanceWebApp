using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Subject
{
    public string SubjectCode { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public int NqfLevel { get; set; }

    public int Credits { get; set; }

    public bool Deprecated { get; set; }

    public string CourseId { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}
