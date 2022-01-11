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
    [Route("api/")]
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
        [Route("GetCourses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            //List<CourseDto> courseDtos = new List<CourseDto>();
            var courses = await _context.Courses.ToListAsync();

            HashSet<CourseDto> courseDtos = new HashSet<CourseDto>();

            foreach (var course in courses)
            {
                courseDtos.Add(new CourseDto
                {
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate
                });
            }

            return courseDtos;
        }
        /// <summary>
        /// Get course by id
        /// </summary>
        /// <param name="id">course identity</param>
        /// <returns>Return course object</returns>
        // GET: api/Courses/5
        [HttpGet]
        [Route("GetCourse/{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }
        /// <summary>
        /// Update course by Id
        /// </summary>
        /// <param name="id">Course Identity</param>
        /// <param name="course">Course object</param>
        /// <returns>Return StatusCode 200 Successfully update course!</returns>
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("UpdateCourse/{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;
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

            return StatusCode(200, "Successfully update course!");
        }
        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="course">Course object</param>
        /// <returns>Return course object that's has been created</returns>
        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateCourse")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (!_context.Courses.Any(c => c.CourseCode == course.CourseCode || c.Name == course.Name || c.Id == course.Id))
            {
                int newId = IdGenerator();
                if (course.Id > 0)
                {
                    return await CreateCourse(course);
                }
                else
                {
                    if (!await _context.Courses.AnyAsync(u => u.Id == newId))
                    {
                        course.Id = newId;
                        return await CreateCourse(course);
                    }
                    else
                    {
                        newId = IdGenerator();
                        course.Id = newId;
                        return await CreateCourse(course);
                    }
                }
            }
            else
            {
                return StatusCode(406, "Course already exist!");
            }
        }
        /// <summary>
        /// Delete course by id
        /// </summary>
        /// <param name="id">Course identity</param>
        /// <returns>Return nothing or return Notfound if course is not exist</returns>
        // DELETE: api/Courses/5
        [HttpDelete]
        [Route("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Successfully delete course");
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int IdGenerator()
        {
            Random idGenerator = new Random();
            return idGenerator.Next(0, int.MaxValue);
        }

        private async Task<ActionResult> CreateCourse(Course course)
        {
            try
            {
                _context.Courses.Add(new Course
                {
                    Id = course.Id,
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                });
                await _context.SaveChangesAsync();
                return StatusCode(200, "Successfully created course");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
