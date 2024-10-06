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
    public class SubjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            return await _context.Subjects
                .Include(s => s.Course) // Include related Course entity
                .Include(s => s.Lessons) // Include related Lessons
                .Include(s => s.StudentCourses) // Include related StudentCourses
                .Include(s => s.StudentSubjects) // Include related StudentSubjects
                .ToListAsync();
        }

        // GET: api/Subject/5
        [HttpGet("{code}")]
        public async Task<ActionResult<Subject>> GetSubject(string code)
        {
            var subject = await _context.Subjects
                .Include(s => s.Course)
                .Include(s => s.Lessons)
                .Include(s => s.StudentCourses)
                .Include(s => s.StudentSubjects)
                .FirstOrDefaultAsync(s => s.SubjectCode == code);

            if (subject == null)
            {
                return NotFound();
            }

            return subject;
        }

        // PUT: api/Subject/5
        [HttpPut("{code}")]
        public async Task<IActionResult> PutSubject(string code, Subject subject)
        {
            if (code != subject.SubjectCode)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(code))
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

        // POST: api/Subject
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubject", new { code = subject.SubjectCode }, subject);
        }

        // DELETE: api/Subject/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteSubject(string code)
        {
            var subject = await _context.Subjects.FindAsync(code);
            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectExists(string code)
        {
            return _context.Subjects.Any(e => e.SubjectCode == code);
        }
    }
}
