using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentAttendanceWebApp.Models;

public partial class Lesson
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
  
    public string Id { get; set; } = null!;

    [Required]
    [BsonElement("lessonDate")]
    [JsonPropertyName("lessonDate")]
    public DateOnly LessonDate { get; set; }

    [Required]
    [BsonElement("startTime")]
    [JsonPropertyName("startTime")]
    public TimeOnly StartTime { get; set; }

    [Required]
    [BsonElement("endTime")]
    [JsonPropertyName("endTime")]
    public TimeOnly EndTime { get; set; }

    [Required]
    [BsonElement("classroomId")]
    [JsonPropertyName("classroomId")]
    public int ClassroomId { get; set; }

    [Required]
    [BsonElement("lecturerId")]
    [JsonPropertyName("lecturerId")]
    public string LecturerId { get; set; } = null!;

    [Required]
    [BsonElement("subjectCode")]
    [JsonPropertyName("subjectCode")]
    public string SubjectCode { get; set; } = null!;

    [BsonElement("students")]
    [JsonPropertyName("students")]
    public List<string>? Students { get; set; }

    [BsonIgnore]
    public virtual ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();

    [BsonIgnore]
    public virtual Classroom? Classroom { get; set; }

    [BsonIgnore]
    public virtual Lecturer? Lecturer { get; set; }

    [BsonIgnore]
    public virtual Subject? SubjectCodeNavigation { get; set; }
}
