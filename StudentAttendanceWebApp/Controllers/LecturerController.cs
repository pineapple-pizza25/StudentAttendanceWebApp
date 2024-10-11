using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : Controller
    {
        private readonly HttpClient _httpClient;

        public LecturerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Lecturer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers()
        {
            var response = await _httpClient.GetAsync("lecturers");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var lecturers = JsonConvert.DeserializeObject<IEnumerable<Lecturer>>(data);

            return Ok(lecturers);
        }

        // GET: api/Lecturer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetLecturer(string id)
        {
            var response = await _httpClient.GetAsync($"lecturers/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var lecturer = JsonConvert.DeserializeObject<Lecturer>(data);

            if (lecturer == null)
            {
                return NotFound();
            }

            return Ok(lecturer);
        }

        // PUT: api/Lecturer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLecturer(string id, Lecturer lecturer)
        {
            if (id != lecturer.LecturerId)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(lecturer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"lecturers/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Lecturer
        [HttpPost]
        public async Task<ActionResult<Lecturer>> PostLecturer(Lecturer lecturer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(lecturer), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("lecturers", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdLecturer = JsonConvert.DeserializeObject<Lecturer>(data);

            return CreatedAtAction("GetLecturer", new { id = createdLecturer.LecturerId }, createdLecturer);
        }

        // DELETE: api/Lecturer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer(string id)
        {
            var response = await _httpClient.DeleteAsync($"lecturers/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }
    }
}
