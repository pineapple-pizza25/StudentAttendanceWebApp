using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Classroom
{
    public int Id { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int CampusId { get; set; }

    public virtual Campus Campus { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
