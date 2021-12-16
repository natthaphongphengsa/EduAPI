using EduAPI.Data;
using EduAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetCourses()
        {
            return Ok(await _context.Courses.ToListAsync());
        }
        [HttpGet]
        [Route("course/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetCourseById(int id)
        {
            var statusCode = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
            if (statusCode == null)
            {
                return StatusCode(204, statusCode);
            }
            return Ok(await _context.Courses.FirstOrDefaultAsync(u => u.Id == id));
        }

    }
}
