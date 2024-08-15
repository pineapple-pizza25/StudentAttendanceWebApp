using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Lecturer
{
    public string LecturerId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public int CampusId { get; set; }

    public virtual Campus Campus { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
