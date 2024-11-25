using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Subject
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("subjectCode", NullValueHandling = NullValueHandling.Ignore)]
        public string SubjectCode { get; set; } = null!;

        [BsonElement("subjectName")]
        [JsonProperty("subjectName", NullValueHandling = NullValueHandling.Ignore)]
        public string SubjectName { get; set; } = null!;

        [BsonElement("nqfLevel")]
        [JsonProperty("nqfLevel", NullValueHandling = NullValueHandling.Ignore)]
        public int NqfLevel { get; set; }

        [BsonElement("credits")]
        [JsonProperty("credits", NullValueHandling = NullValueHandling.Ignore)]
        public int Credits { get; set; }

        [BsonElement("deprecated")]
        [JsonProperty("deprecated", NullValueHandling = NullValueHandling.Ignore)]
        public bool Deprecated { get; set; }

        [BsonElement("courseId")]
        [JsonProperty("courseId", NullValueHandling = NullValueHandling.Ignore)]
        public string CourseId { get; set; } = null!;

        [BsonIgnore]
        [JsonProperty("lessons", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        [BsonIgnore]
        [JsonProperty("studentCourses", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        [BsonIgnore]
        [JsonProperty("studentSubjects", NullValueHandling = NullValueHandling.Ignore)]
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    }
}
