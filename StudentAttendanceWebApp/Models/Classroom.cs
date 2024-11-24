using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    public partial class Classroom
    {
        [JsonProperty("id")]
        public int Id { get; set; } // Unique identifier for the classroom.

        [Required(ErrorMessage = "Room Number is required")]
        [Display(Name = "Room Number")]
        [JsonProperty("roomNumber")]
        public string RoomNumber { get; set; } = null!; // Room number of the classroom.

        [Required(ErrorMessage = "Campus is required")]
        [Display(Name = "Campus")]
        [JsonProperty("campusId")]
        public int CampusId { get; set; } // Foreign key to link the campus.

        [JsonProperty("campus")]
        public virtual Campus Campus { get; set; } = null!; // Navigation property for the Campus.

        [JsonProperty("lessons")]
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>(); // List of lessons associated with this classroom.
    }
}
