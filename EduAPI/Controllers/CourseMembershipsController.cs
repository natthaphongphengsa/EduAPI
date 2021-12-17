using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduAPI.Data;
using EduAPI.Data.Models;

namespace EduAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMembershipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseMembershipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CourseMemberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseMembership>>> GetCourseMemberships()
        {
            return await _context.CourseMemberships.ToListAsync();
        }

        // GET: api/CourseMemberships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseMembership>> GetCourseMembership(int id)
        {
            var courseMembership = await _context.CourseMemberships.FindAsync(id);

            if (courseMembership == null)
            {
                return NotFound();
            }

            return courseMembership;
        }

        // PUT: api/CourseMemberships/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseMembership(int id, CourseMembership courseMembership)
        {
            if (id != courseMembership.Id)
            {
                return BadRequest();
            }

            _context.Entry(courseMembership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseMembershipExists(id))
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

        // POST: api/CourseMemberships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseMembership>> PostCourseMembership(CourseMembership courseMembership)
        {
            _context.CourseMemberships.Add(courseMembership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseMembership", new { id = courseMembership.Id }, courseMembership);
        }

        // DELETE: api/CourseMemberships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseMembership(int id)
        {
            var courseMembership = await _context.CourseMemberships.FindAsync(id);
            if (courseMembership == null)
            {
                return NotFound();
            }

            _context.CourseMemberships.Remove(courseMembership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseMembershipExists(int id)
        {
            return _context.CourseMemberships.Any(e => e.Id == id);
        }
    }
}
