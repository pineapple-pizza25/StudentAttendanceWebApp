using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Lecturer
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("lecturerId", NullValueHandling = NullValueHandling.Ignore)]
        public string LecturerId { get; set; } = null!;

        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; } = null!;

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; } = null!;

        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; } = null!;

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; } = null!;

        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
        public string DateOfBirth { get; set; } = null!;

        [BsonIgnore]
        [JsonProperty("campusId", NullValueHandling = NullValueHandling.Ignore)]
        public int CampusId { get; set; }

        [BsonIgnore]
        [JsonProperty("campus", NullValueHandling = NullValueHandling.Ignore)]
        public Campus Campus { get; set; } = null!;

        [JsonProperty("lessons", NullValueHandling = NullValueHandling.Ignore)]
        public List<Lesson>? Lessons { get; set; } = new List<Lesson>();

       
    }
}
