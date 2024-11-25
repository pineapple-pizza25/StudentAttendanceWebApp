// Course.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    public partial class Course
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Course name is required")]
        [Display(Name = "Course Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters")]
        [JsonProperty("courseName")]
        public string CourseName { get; set; } = null!;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 48, ErrorMessage = "Duration must be between 1 and 48 months")]
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [Display(Name = "Deprecated")]
        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }

        [JsonProperty("subjects")]
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
