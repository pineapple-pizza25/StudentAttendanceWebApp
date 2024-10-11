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
    public class ClassroomController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClassroomController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Classroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            var response = await _httpClient.GetAsync("classrooms");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var classrooms = JsonConvert.DeserializeObject<IEnumerable<Classroom>>(data);

            return Ok(classrooms);
        }

        // GET: api/Classroom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var response = await _httpClient.GetAsync($"classrooms/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var classroom = JsonConvert.DeserializeObject<Classroom>(data);

            if (classroom == null)
            {
                return NotFound();
            }

            return Ok(classroom);
        }

        // PUT: api/Classroom/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassroom(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(classroom), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"classrooms/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Classroom
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom(Classroom classroom)
        {
            var content = new StringContent(JsonConvert.SerializeObject(classroom), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("classrooms", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdClassroom = JsonConvert.DeserializeObject<Classroom>(data);

            return CreatedAtAction("GetClassroom", new { id = createdClassroom.Id }, createdClassroom);
        }

        // DELETE: api/Classroom/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var response = await _httpClient.DeleteAsync($"classrooms/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }
    }
}
