// CourseController.cs
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StudentAttendanceWebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CourseController> _logger;

        public CourseController(HttpClient httpClient, ILogger<CourseController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching all courses");
                var response = await _httpClient.GetAsync("courses");
                return await HandleApiResponse<List<Course>>(response, "Error fetching courses");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error occurred while fetching courses");
            }
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Course ID is required");
            }

            try
            {
                _logger.LogInformation($"Fetching course details for ID: {id}");
                var response = await _httpClient.GetAsync($"courses/{id}");
                return await HandleApiResponse<Course>(response, $"Error fetching course with ID {id}");
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error occurred while fetching course details for ID {id}");
            }
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View(new Course());
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                _logger.LogInformation("Creating new course");
                var json = JsonConvert.SerializeObject(course);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("courses", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Course created successfully");
                    return RedirectToAction(nameof(Index));
                }

                return await HandleApiResponse<Course>(response, "Error creating course");
            }
            catch (Exception ex)
            {
                return HandleException(ex, "Error occurred while creating course");
            }
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Course ID is required");
            }

            try
            {
                _logger.LogInformation($"Fetching course for edit, ID: {id}");
                var response = await _httpClient.GetAsync($"courses/{id}");
                return await HandleApiResponse<Course>(response, $"Error fetching course with ID {id}");
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error occurred while fetching course for edit, ID {id}");
            }
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return View(course);
            }

            try
            {
                _logger.LogInformation($"Updating course with ID: {id}");
                var json = JsonConvert.SerializeObject(course);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"courses/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Course updated successfully");
                    return RedirectToAction(nameof(Index));
                }

                return await HandleApiResponse<Course>(response, $"Error updating course with ID {id}");
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error occurred while updating course with ID {id}");
            }
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Course ID is required");
            }

            try
            {
                _logger.LogInformation($"Fetching course for deletion, ID: {id}");
                var response = await _httpClient.GetAsync($"courses/{id}");
                return await HandleApiResponse<Course>(response, $"Error fetching course with ID {id}");
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error occurred while fetching course for deletion, ID {id}");
            }
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting course with ID: {id}");
                var response = await _httpClient.DeleteAsync($"courses/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Course deleted successfully");
                    return RedirectToAction(nameof(Index));
                }

                return await HandleApiResponse<Course>(response, $"Error deleting course with ID {id}");
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error occurred while deleting course with ID {id}");
            }
        }

        private async Task<IActionResult> HandleApiResponse<T>(HttpResponseMessage response, string errorMessage)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API Response: {jsonResponse}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                ModelState.AddModelError("", $"{errorMessage}: {response.StatusCode} - {response.ReasonPhrase}");
                return View(typeof(T) == typeof(List<Course>) ? new List<Course>() : null);
            }

            try
            {
                var settings = new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        _logger.LogError($"JSON Error: {args.ErrorContext.Error.Message} at path: {args.ErrorContext.Path}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                var result = JsonConvert.DeserializeObject<T>(jsonResponse, settings);

                if (result == null)
                {
                    _logger.LogWarning("Deserialized result is null");
                    ModelState.AddModelError("", "No data received from the server.");
                    return View(typeof(T) == typeof(List<Course>) ? new List<Course>() : null);
                }

                return View(result);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON Processing Error: {ex.Message}");
                ModelState.AddModelError("", "Error processing server response.");
                return View(typeof(T) == typeof(List<Course>) ? new List<Course>() : null);
            }
        }

        private IActionResult HandleException(Exception ex, string message)
        {
            _logger.LogError($"{message}: {ex.GetType().Name} - {ex.Message}");
            ModelState.AddModelError("", $"{message}");
            return View(new List<Course>());
        }
    }
}