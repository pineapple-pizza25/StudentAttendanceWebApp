using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int CampusId { get; set; }
    public bool RegistrationComplete { get; set; }
    public string? image { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Campus Campus { get; set; } = null!;

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}
