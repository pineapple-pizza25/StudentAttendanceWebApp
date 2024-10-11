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
    public class CampusController : Controller
    {
        private readonly HttpClient _httpClient;

        public CampusController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Campus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campus>>> GetCampuses()
        {
            var response = await _httpClient.GetAsync("campuses");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var campuses = JsonConvert.DeserializeObject<IEnumerable<Campus>>(data);

            return Ok(campuses);
        }

        // GET: api/Campus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Campus>> GetCampus(int id)
        {
            var response = await _httpClient.GetAsync($"campuses/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var campus = JsonConvert.DeserializeObject<Campus>(data);

            if (campus == null)
            {
                return NotFound();
            }

            return Ok(campus);
        }

        // PUT: api/Campus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampus(int id, Campus campus)
        {
            if (id != campus.Id)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(campus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"campuses/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Campus
        [HttpPost]
        public async Task<ActionResult<Campus>> PostCampus(Campus campus)
        {
            var content = new StringContent(JsonConvert.SerializeObject(campus), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("campuses", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdCampus = JsonConvert.DeserializeObject<Campus>(data);

            return CreatedAtAction("GetCampus", new { id = createdCampus.Id }, createdCampus);
        }

        // DELETE: api/Campus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampus(int id)
        {
            var response = await _httpClient.DeleteAsync($"campuses/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }
    }
}
