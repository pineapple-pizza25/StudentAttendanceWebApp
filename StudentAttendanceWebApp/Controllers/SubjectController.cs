using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StudentAttendanceWebApp.Controllers
{
    public class SubjectController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SubjectController> _logger;
        private readonly JsonSerializerSettings _jsonSettings;

        public SubjectController(HttpClient httpClient, ILogger<SubjectController> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            _jsonSettings = new JsonSerializerSettings
            {
                Error = HandleDeserializationError,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
            try
            {
                var subjects = await GetSubjectsAsync();
                return View(subjects);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(new List<Subject>());
            }
        }

        // GET: Subject/Details/{code}
        public async Task<IActionResult> Details(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Subject code is required");

            try
            {
                var subject = await GetSubjectByCodeAsync(code);
                if (subject == null)
                    return NotFound($"Subject with code {code} not found");

                return View(subject);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Subject/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Subject subject)
        {
            if (!ModelState.IsValid)
                return View(subject);

            try
            {
                var response = await CreateSubjectAsync(subject);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully created subject: {subject.SubjectName}");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create subject. Please try again.");
                return View(subject);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(subject);
            }
        }

        // GET: Subject/Edit/{code}
        public async Task<IActionResult> Edit(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Subject code is required");

            try
            {
                var subject = await GetSubjectByCodeAsync(code);
                if (subject == null)
                    return NotFound($"Subject with code {code} not found");

                return View(subject);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Subject/Edit/{code}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string code, [FromForm] Subject subject)
        {
            if (code != subject.SubjectCode)
                return BadRequest("Code mismatch");

            if (!ModelState.IsValid)
                return View(subject);

            try
            {
                var response = await UpdateSubjectAsync(code, subject);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully updated subject: {subject.SubjectName}");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update subject. Please try again.");
                return View(subject);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return View(subject);
            }
        }

        // POST: Subject/Delete/{code}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string code)
        {
            try
            {
                var response = await DeleteSubjectAsync(code);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully deleted subject with code: {code}");
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning($"Failed to delete subject with code: {code}");
                return NotFound($"Subject with code {code} not found");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return RedirectToAction(nameof(Index));
            }
        }

       // GET: Subject/Delete/{code}
public async Task<IActionResult> Delete(string code)
{
    if (string.IsNullOrEmpty(code))
        return BadRequest("Subject code is required");

    try
    {
        var subject = await GetSubjectByCodeAsync(code);
        if (subject == null)
            return NotFound($"Subject with code {code} not found");

        return View(subject);
    }
    catch (Exception ex)
    {
        HandleException(ex);
        return RedirectToAction(nameof(Index));
    }
}


        private async Task<List<Subject>> GetSubjectsAsync()
        {
            var response = await _httpClient.GetAsync("subjects");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Subject>>(jsonResponse, _jsonSettings) ?? new List<Subject>();
        }

        private async Task<Subject> GetSubjectByCodeAsync(string code)
        {
            var response = await _httpClient.GetAsync($"subjects/{code}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Subject>(jsonResponse, _jsonSettings);
        }

        private async Task<HttpResponseMessage> CreateSubjectAsync(Subject subject)
        {
            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("subjects", content);
        }

        private async Task<HttpResponseMessage> UpdateSubjectAsync(string code, Subject subject)
        {
            var json = JsonConvert.SerializeObject(subject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"subjects/{code}", content);
        }

        private async Task<HttpResponseMessage> DeleteSubjectAsync(string code)
        {
            return await _httpClient.DeleteAsync($"subjects/{code}");
        }

        private void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        {
            _logger.LogError($"JSON Deserialization Error: {args.ErrorContext.Error.Message} at path: {args.ErrorContext.Path}");
            args.ErrorContext.Handled = true;
        }

        private void HandleException(Exception ex)
        {
            string errorMessage = ex switch
            {
                HttpRequestException httpEx => $"API Connection Error: {httpEx.Message}",
                JsonException jsonEx => $"JSON Processing Error: {jsonEx.Message}",
                _ => $"Unexpected Error: {ex.GetType().Name} - {ex.Message}"
            };

            _logger.LogError(ex, errorMessage);
            ModelState.AddModelError("", errorMessage);
        }

        //#endregion
    }
}
