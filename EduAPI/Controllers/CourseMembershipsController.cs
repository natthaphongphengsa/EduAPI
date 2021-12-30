using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduAPI.Data;
using EduAPI.Data.Models;
using EduAPI.Dtos;

namespace EduAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMembershipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Access to database by ApplicationDbcontext 
        /// </summary>
        /// <param name="context">Database access variabel</param>
        public CourseMembershipsController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all courseMemberships.
        /// </summary>
        /// <returns>return a list of CourseMemberships.</returns>
        // GET: api/CourseMemberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseMembershipDto>>> GetCourseMemberships()
        {
            List<CourseMembershipDto> CourseMembershipList = new List<CourseMembershipDto>();
            foreach (var courseMembership in await _context.CourseMemberships.Include(c => c.Course).Include(u => u.User).ToListAsync())
            {
                CourseMembershipList.Add(new CourseMembershipDto
                {
                    Course = new CourseDto
                    {
                        CourseCode = courseMembership.Course.CourseCode,
                        Name = courseMembership.Course.Name,
                        StartDate = courseMembership.Course.StartDate,
                        EndDate = courseMembership.Course.EndDate
                    },
                    User = new UserDto
                    {
                        FirstName = courseMembership.User.FirstName,
                        LastName = courseMembership.User.LastName,
                        Email = courseMembership.User.Email
                    },
                    EndrolledDate = courseMembership.EndrolledDate,
                });
            }
            return CourseMembershipList;
        }
        /// <summary>
        /// Get one courseMemnerships by Id.
        /// </summary>
        /// <param name="id">courseMembership identity Id.</param>
        /// <returns>Return one CourseMembership.</returns>
        // GET: api/CourseMemberships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseMembership>> GetCourseMembership(int id)
        {
            var courseMembership = await _context.CourseMemberships.Include(c => c.Course).Include(u => u.User).FirstOrDefaultAsync(c => c.Id == id);

            if (courseMembership == null)
            {
                return NotFound();
            }

            return courseMembership;
        }
        /// <summary>
        /// Update CourseMembership by find the Id.
        /// </summary>
        /// <param name="id">Identity for CourseMembership.</param>
        /// <param name="courseMembership">courseMembership object.</param>
        /// <returns>return nothing.</returns>
        // PUT: api/CourseMemberships/5
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
        [HttpPost]
        public async Task<ActionResult<CourseMembershipDto>> PostCourseMembership(string userEmail, string courseCode, DateTime endRolledDate)
        {
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            User user = await _context.Users.FirstOrDefaultAsync(c => c.Email == userEmail);

            CourseMembership courseMembership = new CourseMembership()
            {
                Course = course,
                User = user,
                EndrolledDate = endRolledDate
            };
            await _context.CourseMemberships.AddAsync(courseMembership);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourseMembership", new
            {
                id = courseMembership.Id
            }, courseMembership);
        }
        /// <summary>
        /// Delete CouseMembership by find the Id
        /// </summary>
        /// <param name="id">Identity for CourseMembership</param>
        /// <returns>return nothing</returns>
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
        /// <summary>
        /// Check CouseMembership by Id, if it's exist
        /// then return true or false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CourseMembershipExists(int id)
        {
            return _context.CourseMemberships.Any(e => e.Id == id);
        }
    }
}
