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
    public class AdministratorController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdministratorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        // GET: api/Administrator
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
            var response = await _httpClient.GetAsync("administrators");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var administrators = JsonConvert.DeserializeObject<IEnumerable<Administrator>>(data);

            // Return API data as JSON
            return Ok(administrators);
        }

        // GET: api/Administrator/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Administrator>> GetAdministrator(string id)
        {
            var response = await _httpClient.GetAsync($"administrators/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var administrator = JsonConvert.DeserializeObject<Administrator>(data);

            if (administrator == null)
            {
                return NotFound();
            }

            // Return API data as JSON
            return Ok(administrator);
        }

        // PUT: api/Administrator/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministrator(string id, Administrator administrator)
        {
            if (id != administrator.AdministratorId)
            {
                return BadRequest();
            }

            var content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"administrators/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // POST: api/Administrator
        [HttpPost]
        public async Task<ActionResult<Administrator>> PostAdministrator(Administrator administrator)
        {
            var content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("administrators", content);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var createdAdministrator = JsonConvert.DeserializeObject<Administrator>(data);

            return CreatedAtAction("GetAdministrator", new { id = createdAdministrator.AdministratorId }, createdAdministrator);
        }

        // DELETE: api/Administrator/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrator(string id)
        {
            var response = await _httpClient.DeleteAsync($"administrators/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return NoContent();
        }

        // Now we add support for rendering views

        // GET: Administrator/Details/5
        [HttpGet("/Administrator/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetAsync($"administrators/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var administrator = JsonConvert.DeserializeObject<Administrator>(data);

            if (administrator == null)
            {
                return NotFound();
            }

            // Return a view and pass the administrator model
            return View(administrator);
        }

        // GET: Administrator/Create
        [HttpGet("/Administrator/Create")]
        public IActionResult Create()
        {
            // Return the form view for creating a new Administrator
            return View();
        }

        // POST: Administrator/Create
        [HttpPost("/Administrator/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdministratorId,Name,Campus")] Administrator administrator)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("administrators", content);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
                }

                return RedirectToAction(nameof(Index)); // Redirect to the list page after successful creation
            }

            // If the model state is not valid, return the same form view with the validation messages
            return View(administrator);
        }

        // GET: Administrator/Edit/5
        [HttpGet("/Administrator/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"administrators/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var data = await response.Content.ReadAsStringAsync();
            var administrator = JsonConvert.DeserializeObject<Administrator>(data);

            if (administrator == null)
            {
                return NotFound();
            }

            // Return the form view for editing the Administrator
            return View(administrator);
        }

        // POST: Administrator/Edit/5
        [HttpPost("/Administrator/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AdministratorId,Name,Campus")] Administrator administrator)
        {
            if (id != administrator.AdministratorId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(administrator), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"administrators/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
                }

                return RedirectToAction(nameof(Index)); // Redirect to the list page after successful editing
            }

            // If the model state is not valid, return the same form view with the validation messages
            return View(administrator);
        }
    }
}
