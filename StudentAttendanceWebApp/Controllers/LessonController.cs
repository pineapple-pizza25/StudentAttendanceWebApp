using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudentAttendanceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    public class LessonController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseRoute = "lessons";

        public LessonController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://faceon-api.calmwave-03f9df68.southafricanorth.azurecontainerapps.io/api/");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiBaseRoute);
                return response.IsSuccessStatusCode
                    ? View(JsonConvert.DeserializeObject<IEnumerable<Lesson>>(await response.Content.ReadAsStringAsync()))
                    : StatusCode((int)response.StatusCode, "Error retrieving lessons.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseRoute}/{id}");
                return response.IsSuccessStatusCode
                    ? View(JsonConvert.DeserializeObject<Lesson>(await response.Content.ReadAsStringAsync()))
                    : StatusCode((int)response.StatusCode, "Error retrieving lesson details.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson)
        {
            if (!ModelState.IsValid) return View(lesson);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lesson), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiBaseRoute, content);

                return response.IsSuccessStatusCode
                    ? RedirectToAction(nameof(Index))
                    : StatusCode((int)response.StatusCode, "Error creating lesson.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseRoute}/{id}");
                return response.IsSuccessStatusCode
                    ? View(JsonConvert.DeserializeObject<Lesson>(await response.Content.ReadAsStringAsync()))
                    : StatusCode((int)response.StatusCode, "Error retrieving lesson.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Lesson lesson)
        {
            if (id != lesson.Id) return BadRequest();
            if (!ModelState.IsValid) return View(lesson);

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(lesson), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{ApiBaseRoute}/{id}", content);

                return response.IsSuccessStatusCode
                    ? RedirectToAction(nameof(Index))
                    : StatusCode((int)response.StatusCode, "Error updating lesson.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseRoute}/{id}");
                return response.IsSuccessStatusCode
                    ? View(JsonConvert.DeserializeObject<Lesson>(await response.Content.ReadAsStringAsync()))
                    : StatusCode((int)response.StatusCode, "Error retrieving lesson.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiBaseRoute}/{id}");
                return response.IsSuccessStatusCode
                    ? RedirectToAction(nameof(Index))
                    : StatusCode((int)response.StatusCode, "Error deleting lesson.");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
        }
    }
}