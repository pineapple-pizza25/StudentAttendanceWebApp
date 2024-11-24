using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClassroomController> _logger;

        public ClassroomController(HttpClient httpClient, ILogger<ClassroomController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<SelectList> GetCampusSelectListAsync(int? selectedCampusId = null)
        {
            try
            {
                var response = await _httpClient.GetAsync("campuses");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var campuses = JsonConvert.DeserializeObject<List<Campus>>(jsonResponse);
                    return new SelectList(campuses, "Id", "Name", selectedCampusId);
                }
                _logger.LogError($"Error fetching campuses: {response.StatusCode}");
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while fetching campuses: {ex.Message}");
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }

        // GET: Classroom
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching all classrooms");
                var response = await _httpClient.GetAsync("classrooms?includeCampus=true");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = $"Error fetching classrooms: {response.StatusCode}";
                    _logger.LogError($"API Error: {errorMessage}");
                    TempData["Error"] = errorMessage;
                    return View(new List<Classroom>());
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var classrooms = JsonConvert.DeserializeObject<List<Classroom>>(jsonResponse);
                return View(classrooms ?? new List<Classroom>());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index: {ex.Message}");
                TempData["Error"] = "Error fetching classrooms. Please try again later.";
                return View(new List<Classroom>());
            }
        }

        // GET: Classroom/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"classrooms/{id}?includeCampus=true&includeLessons=true");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API Error: {response.StatusCode} - {response.ReasonPhrase}");
                    TempData["Error"] = "Classroom not found.";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var classroom = JsonConvert.DeserializeObject<Classroom>(jsonResponse);
                return View(classroom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Details: {ex.Message}");
                TempData["Error"] = "Error retrieving classroom details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Classroom/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Campuses = await GetCampusSelectListAsync();
                return View(new Classroom());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create GET: {ex.Message}");
                TempData["Error"] = "Error loading create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Classroom/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomNumber,CampusId")] Classroom classroom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(classroom);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("classrooms", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Classroom created successfully.";
                        return RedirectToAction(nameof(Index));
                    }

                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API Error: {response.StatusCode} - {errorResponse}");
                }

                ViewBag.Campuses = await GetCampusSelectListAsync(classroom.CampusId);
                return View(classroom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create POST: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                ViewBag.Campuses = await GetCampusSelectListAsync(classroom.CampusId);
                return View(classroom);
            }
        }

        // GET: Classroom/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"classrooms/{id}?includeCampus=true");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Classroom not found.";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var classroom = JsonConvert.DeserializeObject<Classroom>(jsonResponse);
                ViewBag.Campuses = await GetCampusSelectListAsync(classroom.CampusId);
                return View(classroom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Edit GET: {ex.Message}");
                TempData["Error"] = "Error loading edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Classroom/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomNumber,CampusId")] Classroom classroom)
        {
            if (id != classroom.Id)
            {
                TempData["Error"] = "Invalid classroom ID.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(classroom);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"classrooms/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "Classroom updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }

                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"API Error: {response.StatusCode} - {errorResponse}");
                }

                ViewBag.Campuses = await GetCampusSelectListAsync(classroom.CampusId);
                return View(classroom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Edit POST: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                ViewBag.Campuses = await GetCampusSelectListAsync(classroom.CampusId);
                return View(classroom);
            }
        }

        // GET: Classroom/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"classrooms/{id}?includeCampus=true&includeLessons=true");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Classroom not found.";
                    return RedirectToAction(nameof(Index));
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var classroom = JsonConvert.DeserializeObject<Classroom>(jsonResponse);
                return View(classroom);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete GET: {ex.Message}");
                TempData["Error"] = "Error loading delete confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Classroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"classrooms/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Classroom deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Error deleting classroom: {response.StatusCode} - {errorResponse}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteConfirmed: {ex.Message}");
                TempData["Error"] = "An unexpected error occurred while deleting the classroom.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}