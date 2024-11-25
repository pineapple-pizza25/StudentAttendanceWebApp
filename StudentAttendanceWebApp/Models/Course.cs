// Course.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    public partial class Course
    {
        [JsonProperty("courseId")]
        public string CourseId { get; set; } = null!;

        [Required(ErrorMessage = "Course name is required")]
        [Display(Name = "Course Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        [JsonProperty("courseName")]
        public string CourseName { get; set; } = null!;

        [Display(Name = "Deprecated")]
        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("subjects")]
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
