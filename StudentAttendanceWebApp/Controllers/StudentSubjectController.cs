using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    [Route("StudentSubject")]
    public class StudentSubjectController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentSubjectController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: StudentSubject
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("studentsubjects");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error"); // Return an error view if the API call fails
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentSubjects = JsonConvert.DeserializeObject<IEnumerable<StudentSubject>>(data);

            return View(studentSubjects); // Return the data to a view
        }

        // GET: StudentSubject/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"studentsubjects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentSubject = JsonConvert.DeserializeObject<StudentSubject>(data);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject); // Return the details view
        }

        // GET: StudentSubject/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentSubject/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,SubjectCode")] StudentSubject studentSubject)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(studentSubject), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("studentsubjects", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to index after successful creation
                }
            }

            return View(studentSubject);
        }

        // GET: StudentSubject/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"studentsubjects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentSubject = JsonConvert.DeserializeObject<StudentSubject>(data);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject); // Return the edit view
        }

        // POST: StudentSubject/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,SubjectCode")] StudentSubject studentSubject)
        {
            if (id != studentSubject.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(studentSubject), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"studentsubjects/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(studentSubject);
        }

        // GET: StudentSubject/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"studentsubjects/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentSubject = JsonConvert.DeserializeObject<StudentSubject>(data);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return View(studentSubject); // Return the delete view
        }

        // POST: StudentSubject/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"studentsubjects/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }
    }
}
