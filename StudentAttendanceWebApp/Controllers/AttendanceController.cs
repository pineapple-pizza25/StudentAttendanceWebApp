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
    public class AttendanceController : Controller
    {
        private readonly HttpClient _httpClient;

        public AttendanceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            var response = await _httpClient.GetAsync("attendances");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var attendances = JsonConvert.DeserializeObject<IEnumerable<Attendance>>(data);

            return Ok(attendances);
        }

        // GET: api/Attendance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var response = await _httpClient.GetAsync($"attendances/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var attendance = JsonConvert.DeserializeObject<Attendance>(data);

            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/Attendance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(attendance), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"attendances/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Attendance
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
        {
            var content = new StringContent(JsonConvert.SerializeObject(attendance), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("attendances", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdAttendance = JsonConvert.DeserializeObject<Attendance>(data);

            return CreatedAtAction("GetAttendance", new { id = createdAttendance.Id }, createdAttendance);
        }

        // DELETE: api/Attendance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var response = await _httpClient.DeleteAsync($"attendances/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }
    }
}
