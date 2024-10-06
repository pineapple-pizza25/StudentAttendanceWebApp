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
    public class CampusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CampusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Campus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campus>>> GetCampuses()
        {
            return await _context.Campuses
                .Include(c => c.Administrators)
                .Include(c => c.Classrooms)
                .Include(c => c.Lecturers)
                .Include(c => c.Students)
                .ToListAsync();
        }

        // GET: api/Campus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Campus>> GetCampus(int id)
        {
            var campus = await _context.Campuses
                .Include(c => c.Administrators)
                .Include(c => c.Classrooms)
                .Include(c => c.Lecturers)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campus == null)
            {
                return NotFound();
            }

            return campus;
        }

        // PUT: api/Campus/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampus(int id, Campus campus)
        {
            if (id != campus.Id)
            {
                return BadRequest();
            }

            _context.Entry(campus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampusExists(id))
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

        // POST: api/Campus
        [HttpPost]
        public async Task<ActionResult<Campus>> PostCampus(Campus campus)
        {
            _context.Campuses.Add(campus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCampus", new { id = campus.Id }, campus);
        }

        // DELETE: api/Campus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampus(int id)
        {
            var campus = await _context.Campuses.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }

            _context.Campuses.Remove(campus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampusExists(int id)
        {
            return _context.Campuses.Any(e => e.Id == id);
        }
    }
}
