using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: /Student
        public async Task<IActionResult> Index()
        {
            System.Diagnostics.Debug.WriteLine("We're testing this controller");

            try
            {
                var requestUri = _httpClient.BaseAddress + "Student";
                System.Diagnostics.Debug.WriteLine($"Requesting: {requestUri}");

                var response = await _httpClient.GetAsync("Student");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    System.Diagnostics.Debug.WriteLine(jsonResponse);

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        var students = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);

                        System.Diagnostics.Debug.WriteLine(students);

                        return View(students);
                    }
                }

                ModelState.AddModelError("", "Could not retrieve data from the API.");
                return View(new List<Student>());
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Exception: {ex.Message}");
                ModelState.AddModelError("", $"An error occurred while sending the request: {ex.Message}");
                return View(new List<Student>());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(new List<Student>());
            }
        }

        // GET: /Student/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync($"students/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);
                return View(student);
            }
            return NotFound();
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,PhoneNumber,Email,DateOfBirth,CampusId")] Student student)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(student);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("students", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(student);
        }

        // GET: /Student/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"students/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);
                return View(student);
            }
            return NotFound();
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentId,FirstName,LastName,PhoneNumber,Email,DateOfBirth,CampusId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var jsonContent = JsonConvert.SerializeObject(student);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"students/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(student);
        }

        // GET: /Student/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.GetAsync($"students/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(jsonResponse);
                return View(student);
            }
            return NotFound();
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var response = await _httpClient.DeleteAsync($"students/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
