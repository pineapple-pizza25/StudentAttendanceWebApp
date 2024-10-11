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
    public class StudentCourseController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentCourseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/StudentCourse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
            var response = await _httpClient.GetAsync("studentcourses");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentCourses = JsonConvert.DeserializeObject<IEnumerable<StudentCourse>>(data);

            return Ok(studentCourses);
        }

        // GET: api/StudentCourse/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(int id)
        {
            var response = await _httpClient.GetAsync($"studentcourses/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var studentCourse = JsonConvert.DeserializeObject<StudentCourse>(data);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return Ok(studentCourse);
        }

        // PUT: api/StudentCourse/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse(int id, StudentCourse studentCourse)
        {
            if (id != studentCourse.Id)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(studentCourse), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"studentcourses/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/StudentCourse
        [HttpPost]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(StudentCourse studentCourse)
        {
            var content = new StringContent(JsonConvert.SerializeObject(studentCourse), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("studentcourses", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdStudentCourse = JsonConvert.DeserializeObject<StudentCourse>(data);

            return CreatedAtAction("GetStudentCourse", new { id = createdStudentCourse.Id }, createdStudentCourse);
        }

        // DELETE: api/StudentCourse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourse(int id)
        {
            var response = await _httpClient.DeleteAsync($"studentcourses/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }
    }
}
