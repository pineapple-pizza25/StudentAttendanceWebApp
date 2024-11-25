using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace StudentAttendanceWebApp.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [JsonProperty("studentId", NullValueHandling = NullValueHandling.Ignore)]     
        public string StudentId { get; set; } = null!;


        [BsonElement("password")]
        public string? Password { get; set; }

        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]      
        public string FirstName { get; set; } = null!;

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]      
        public string LastName { get; set; } = null!;


        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
       public string Username { get; set; } = null!;


        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; } = null!;


        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]      
        public string Email { get; set; } = null!;


        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]      
        public string DateOfBirth { get; set; } = null!;


        [BsonIgnore]
        [JsonProperty("campusId", NullValueHandling = NullValueHandling.Ignore)]       
        public int CampusId { get; set; }


        [JsonProperty("registrationComplete", NullValueHandling = NullValueHandling.Ignore)]     
        public bool RegistrationComplete { get; set; }


        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]       
        public string Image { get; set; } = null!;


        [JsonProperty("subjects", NullValueHandling = NullValueHandling.Ignore)]    
        public List<string>? Subjects { get; set; } = new List<string>();

        [JsonProperty("attendance", NullValueHandling = NullValueHandling.Ignore)]
        public List<Attendance>? Attendance { get; set; } = new List<Attendance>();

    }
}