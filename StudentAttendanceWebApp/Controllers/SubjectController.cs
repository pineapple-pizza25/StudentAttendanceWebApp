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
    public class SubjectController : Controller
    {
        private readonly HttpClient _httpClient;

        public SubjectController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var response = await _httpClient.GetAsync("subjects");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error retrieving subjects from API");
            }

            var data = await response.Content.ReadAsStringAsync();
            var subjects = JsonConvert.DeserializeObject<IEnumerable<Subject>>(data);

            return Ok(subjects); // Return the list of subjects
        }

        // GET: api/Subject/5
        [HttpGet("{code}")]
        public async Task<IActionResult> GetSubject(string code)
        {
            var response = await _httpClient.GetAsync($"subjects/{code}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error retrieving the subject");
            }

            var data = await response.Content.ReadAsStringAsync();
            var subject = JsonConvert.DeserializeObject<Subject>(data);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject); // Return the subject details
        }

        // PUT: api/Subject/5
        [HttpPut("{code}")]
        public async Task<IActionResult> PutSubject(string code, Subject subject)
        {
            if (code != subject.SubjectCode)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"subjects/{code}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error updating the subject");
            }

            return NoContent();
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> PostSubject(Subject subject)
        {
            var content = new StringContent(JsonConvert.SerializeObject(subject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("subjects", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error creating the subject");
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdSubject = JsonConvert.DeserializeObject<Subject>(data);

            return CreatedAtAction(nameof(GetSubject), new { code = createdSubject.SubjectCode }, createdSubject);
        }

        // DELETE: api/Subject/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteSubject(string code)
        {
            var response = await _httpClient.DeleteAsync($"subjects/{code}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error deleting the subject");
            }

            return NoContent();
        }
    }
}
