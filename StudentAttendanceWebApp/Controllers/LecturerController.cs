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
    public class LecturerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LecturerController> _logger;

        public LecturerController(HttpClient httpClient, ILogger<LecturerController> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: Lecturer
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("lecturers");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var lecturers = JsonConvert.DeserializeObject<IEnumerable<Lecturer>>(data);
                return View(lecturers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lecturers");
                return View(new List<Lecturer>());
            }
        }

        // GET: Lecturer/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Lecturer ID is required");

            try
            {
                var response = await _httpClient.GetAsync($"lecturers/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound($"Lecturer with ID {id} not found");

                var data = await response.Content.ReadAsStringAsync();
                var lecturer = JsonConvert.DeserializeObject<Lecturer>(data);
                return View(lecturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lecturer details");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Lecturer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
                return View(lecturer);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lecturer), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("lecturers", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Failed to create lecturer. Please try again.");
                return View(lecturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lecturer");
                return View(lecturer);
            }
        }

        // GET: Lecturer/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Lecturer ID is required");

            try
            {
                var response = await _httpClient.GetAsync($"lecturers/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound($"Lecturer with ID {id} not found");

                var data = await response.Content.ReadAsStringAsync();
                var lecturer = JsonConvert.DeserializeObject<Lecturer>(data);
                return View(lecturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lecturer for editing");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Lecturer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Lecturer lecturer)
        {
            if (id != lecturer.LecturerId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return View(lecturer);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lecturer), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"lecturers/{id}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Failed to update lecturer. Please try again.");
                return View(lecturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lecturer");
                return View(lecturer);
            }
        }

        // GET: Lecturer/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Lecturer ID is required");

            try
            {
                var response = await _httpClient.GetAsync($"lecturers/{id}");
                if (!response.IsSuccessStatusCode)
                    return NotFound($"Lecturer with ID {id} not found");

                var data = await response.Content.ReadAsStringAsync();
                var lecturer = JsonConvert.DeserializeObject<Lecturer>(data);
                return View(lecturer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching lecturer for deletion");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Lecturer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"lecturers/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                _logger.LogWarning($"Failed to delete lecturer with ID: {id}");
                ModelState.AddModelError("", $"Failed to delete lecturer with ID {id}. Please try again.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting lecturer");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
