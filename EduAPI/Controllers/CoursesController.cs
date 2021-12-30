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
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns>return a list of courses</returns>
        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            List<CourseDto> courseList = new List<CourseDto>();
            foreach (var course in await _context.Courses.ToListAsync())
            {
                courseList.Add(new CourseDto
                {
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                });
            }
            return courseList;
        }
        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="id">course identity</param>
        /// <returns>Return course object</returns>
        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return new CourseDto
            {
                Name = course.Name,
                CourseCode = course.CourseCode,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
        }
        /// <summary>
        /// Update course by Id
        /// </summary>
        /// <param name="id">Course Identity</param>
        /// <param name="course">Course object</param>
        /// <returns>Return nothing if exist else return BadRequest</returns>
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseDto course)
        {
            //if (id != course.Id)
            //{
            //    return BadRequest();
            //}

            _context.Entry(
                new Course
                {
                    Id = id,
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                }).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="course">Course object</param>
        /// <returns>Return course object that's has been created</returns>
        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto course)
        {
            Course newCourse = new Course()
            {
                CourseCode = course.CourseCode,
                Name = course.Name,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = newCourse.Id }, newCourse);
        }
        /// <summary>
        /// Delete course by id
        /// </summary>
        /// <param name="id">Course identity</param>
        /// <returns>Return nothing or return Notfound if course is not exist</returns>
        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// check if course is exist by id
        /// </summary>
        /// <param name="id">course Identity</param>
        /// <returns>Return true or false if course exist or not</returns>
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
