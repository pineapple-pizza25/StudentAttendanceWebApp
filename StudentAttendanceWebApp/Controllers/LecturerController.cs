using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendanceWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAttendanceWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lecturer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lecturer>>> GetLecturers()
        {
            return await _context.Lecturers
                .Include(l => l.Campus) // Include related Campus entity
                .Include(l => l.Lessons) // Include related Lessons collection
                .ToListAsync();
        }

        // GET: api/Lecturer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lecturer>> GetLecturer(string id)
        {
            var lecturer = await _context.Lecturers
                .Include(l => l.Campus)
                .Include(l => l.Lessons)
                .FirstOrDefaultAsync(l => l.LecturerId == id);

            if (lecturer == null)
            {
                return NotFound();
            }

            return lecturer;
        }

        // PUT: api/Lecturer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLecturer(string id, Lecturer lecturer)
        {
            if (id != lecturer.LecturerId)
            {
                return BadRequest();
            }

            _context.Entry(lecturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Lecturer
        [HttpPost]
        public async Task<ActionResult<Lecturer>> PostLecturer(Lecturer lecturer)
        {
            _context.Lecturers.Add(lecturer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLecturer", new { id = lecturer.LecturerId }, lecturer);
        }

        // DELETE: api/Lecturer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer(string id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LecturerExists(string id)
        {
            return _context.Lecturers.Any(e => e.LecturerId == id);
        }
    }
}
