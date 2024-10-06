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
    public class StudentSubjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentSubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentSubject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentSubject>>> GetStudentSubjects()
        {
            return await _context.StudentSubjects
                .Include(ss => ss.Student) // Include related Student entity
                .Include(ss => ss.SubjectCodeNavigation) // Include related Subject entity
                .ToListAsync();
        }

        // GET: api/StudentSubject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentSubject>> GetStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.SubjectCodeNavigation)
                .FirstOrDefaultAsync(ss => ss.Id == id);

            if (studentSubject == null)
            {
                return NotFound();
            }

            return studentSubject;
        }

        // PUT: api/StudentSubject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentSubject(int id, StudentSubject studentSubject)
        {
            if (id != studentSubject.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentSubjectExists(id))
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

        // POST: api/StudentSubject
        [HttpPost]
        public async Task<ActionResult<StudentSubject>> PostStudentSubject(StudentSubject studentSubject)
        {
            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentSubject", new { id = studentSubject.Id }, studentSubject);
        }

        // DELETE: api/StudentSubject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentSubject(int id)
        {
            var studentSubject = await _context.StudentSubjects.FindAsync(id);
            if (studentSubject == null)
            {
                return NotFound();
            }

            _context.StudentSubjects.Remove(studentSubject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentSubjectExists(int id)
        {
            return _context.StudentSubjects.Any(e => e.Id == id);
        }
    }
}
