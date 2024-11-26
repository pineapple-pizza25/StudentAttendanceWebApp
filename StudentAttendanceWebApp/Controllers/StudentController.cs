using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Linq;
using Azure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceWebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StudentController> _logger;
        private readonly JsonSerializerSettings _jsonSettings;

        public StudentController(HttpClient httpClient, ILogger<StudentController> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            _jsonSettings = new JsonSerializerSettings
            {
                Error = HandleDeserializationError,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        // GET: /Student
        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await GetStudentsAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(new List<Student>());
            }
        }

        // GET: /Student/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Student ID is required");
            }

            try
            {
                var student = await GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound($"Student with ID {id} not found");
                }

                return View(student);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                var response = await CreateStudentAsync(student);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully created student: {student.FirstName} {student.LastName}");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create student. Please try again.");
                return View(student);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(student);
            }
        }

        // POST: /Student/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, [FromForm] Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                var response = await UpdateStudentAsync(id, student);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully updated student: {student.FirstName} {student.LastName}");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update student. Please try again.");
                return View(student);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(student);
            }
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await DeleteStudentAsync(id);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully deleted student with ID: {id}");
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning($"Failed to delete student with ID: {id}");
                return NotFound($"Student with ID {id} not found");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteRegistration(string studentId)
        {
            try
            {
                // Log the start of the registration completion attempt
                _logger.LogInformation("Attempting to complete registration for student {StudentId}", studentId);
                System.Diagnostics.Debug.WriteLine("Attempting to complete registration for student {StudentId}");
                var response = await _httpClient.PatchAsync(
                    $"students/{studentId}/complete-registration",
                    new StringContent(JsonConvert.SerializeObject(new { studentId }), Encoding.UTF8, "application/json")
                );
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully completed registration for student {StudentId}", studentId);
                    System.Diagnostics.Debug.WriteLine("Successfully completed registration for student {StudentId}", studentId);
                    var result = JsonConvert.DeserializeObject<dynamic>(content, _jsonSettings);
                    return RedirectToAction(nameof(Index));
                }
                // Log the error response
                _logger.LogWarning("Failed to complete registration for student {StudentId}. Status: {StatusCode}, Response: {Response}",
                    studentId, response.StatusCode, content);
                System.Diagnostics.Debug.WriteLine("Attempting to complete registration for student {StudentId}");
                // Handle different status codes
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.NotFound => NotFound("Student not found"),
                    System.Net.HttpStatusCode.BadRequest => BadRequest(content),
                    _ => StatusCode((int)response.StatusCode, "Failed to complete registration")
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while completing registration for student {StudentId}", studentId);
                return StatusCode(503, "Service temporarily unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while completing registration for student {StudentId}", studentId);
                return StatusCode(500, "An unexpected error occurred");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Attendance(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                ModelState.AddModelError("", "Student ID is required.");
                return View();
            }

            var url = $"/api/students/attendance/{studentId}"; // Replace with your API base URL if necessary.

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var attendanceRecords = await response.Content.ReadFromJsonAsync<List<Attendance>>();
                     if (attendanceRecords == null || !attendanceRecords.Any())
                    {
                        ModelState.AddModelError("", "No attendance records found for this student.");
                        return View(new List<Attendance>());
                    }

                    return View("Attendance", attendanceRecords);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", errorMessage);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching attendance: " + ex.Message);
            }

            return View("Attendance", new List<Attendance>());
        }

        #region Private Methods

        private async Task<List<Student>> GetStudentsAsync()
        {
            var response = await _httpClient.GetAsync("students");
            await LogResponseDetails(response);

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var students = JsonConvert.DeserializeObject<List<Student>>(jsonResponse, _jsonSettings);
            return students ?? new List<Student>();
        }

        private async Task<Student> GetStudentByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"students/{id}");
            await LogResponseDetails(response);

            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Student>(jsonResponse, _jsonSettings);
        }

        private async Task<HttpResponseMessage> CreateStudentAsync(Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("students", content);
        }

        private async Task<HttpResponseMessage> UpdateStudentAsync(string id, Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"students/{id}", content);
        }

        private async Task<HttpResponseMessage> DeleteStudentAsync(string id)
        {
            return await _httpClient.DeleteAsync($"students/{id}");
        }

        private async Task LogResponseDetails(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API Response - Status: {response.StatusCode}, Content: {content}");
        }

        private void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        {
            _logger.LogError($"JSON Deserialization Error: {args.ErrorContext.Error.Message} at path: {args.ErrorContext.Path}");
            args.ErrorContext.Handled = true;
        }

        private void HandleException(Exception ex)
        {
            string errorMessage = ex switch
            {
                HttpRequestException httpEx => $"API Connection Error: {httpEx.Message}",
                JsonException jsonEx => $"JSON Processing Error: {jsonEx.Message}",
                _ => $"Unexpected Error: {ex.GetType().Name} - {ex.Message}"
            };

            _logger.LogError(ex, errorMessage);
            ModelState.AddModelError("", errorMessage);
        }

        #endregion
    }
}