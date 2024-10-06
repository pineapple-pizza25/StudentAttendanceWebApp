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
    public class StudentCourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourse>>> GetStudentCourses()
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student) // Include related Student entity
                .Include(sc => sc.ModuleCodeNavigation) // Include related Subject
                .ToListAsync();
        }

        // GET: api/StudentCourse/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourse>> GetStudentCourse(int id)
        {
            var studentCourse = await _context.StudentCourses
                .Include(sc => sc.Student)
                .Include(sc => sc.ModuleCodeNavigation)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (studentCourse == null)
            {
                return NotFound();
            }

            return studentCourse;
        }

        // PUT: api/StudentCourse/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse(int id, StudentCourse studentCourse)
        {
            if (id != studentCourse.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(id))
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

        // POST: api/StudentCourse
        [HttpPost]
        public async Task<ActionResult<StudentCourse>> PostStudentCourse(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourse", new { id = studentCourse.Id }, studentCourse);
        }

        // DELETE: api/StudentCourse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourse(int id)
        {
            var studentCourse = await _context.StudentCourses.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCourseExists(int id)
        {
            return _context.StudentCourses.Any(e => e.Id == id);
        }
    }
}
