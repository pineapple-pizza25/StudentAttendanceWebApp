using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StudentAttendanceWebApp.Models;

public partial class Attendance
{
    [JsonProperty("lessonId", NullValueHandling = NullValueHandling.Ignore)]
    public string LessonId { get; set; } = null!;

    [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? Timestamp { get; set; }

    [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
    public string Status { get; set; } = null!;
}
