﻿using System;
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
    public class CourseMembershipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CourseMembershipsController(ApplicationDbContext context)
        {
            _context = context;
            //users = (List<string>)_context.Users.ToList().Select(u => u.FirstName);
        }
        /// <summary>
        /// Get all courseMemberships.
        /// </summary>
        /// <returns>return a list of CourseMemberships.</returns>
        // GET: api/CourseMemberships
        [HttpGet]
        [Route("GetCourseMemberships")]
        public async Task<ActionResult<IEnumerable<CourseMembership>>> GetCourseMemberships()
        {
            return await _context.CourseMemberships.Include(c => c.Course).Include(u => u.User).ToListAsync();
        }
        /// <summary>
        /// Create coursemembership
        /// </summary>
        /// <param name="courseMembershipDto">courseMembership object</param>
        /// <returns></returns>
        // POST: api/CourseMemberships
        [HttpPost]
        [Route("CreateCourseMembership")]
        public async Task<ActionResult<CourseMembership>> PostCourseMembership(CourseMembershipDto courseMembershipDto)
        {
            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseMembershipDto.Course.CourseCode);
            User user = await _context.Users.FirstOrDefaultAsync(c => c.Email == courseMembershipDto.User.Email);

            if (course != null && user != null)
            {
                CourseMembership courseMembership = new CourseMembership()
                {
                    Course = course,
                    User = user,
                    EndrolledDate = courseMembershipDto.EndrolledDate
                };

                if (!await _context.CourseMemberships.AnyAsync(c => c.User == user && c.Course == course))
                {
                    await _context.CourseMemberships.AddAsync(courseMembership);
                    await _context.SaveChangesAsync();
                    return StatusCode(200, "Successfully created user");
                }
                else
                {
                    return StatusCode(406, "CourseMemberships allready exist!");
                }

                //return CreatedAtAction("GetCourseMembership", new { id = courseMembership.Id }, courseMembership);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get one courseMemnerships by Id.
        /// </summary>
        /// <param name="id">courseMembership identity Id.</param>
        /// <returns>Return one CourseMembership.</returns>
        // GET: api/CourseMemberships/5
        [HttpGet]
        [Route("GetCourseMembership/{id}")]
        public async Task<ActionResult<CourseMembershipDto>> GetCourseMembership(int id)
        {
            var courseMembership = await _context.CourseMemberships.Include(c => c.Course).Include(u => u.User).FirstOrDefaultAsync(c => c.Id == id);

            if (courseMembership != null)
            {
                CourseMembershipDto courseMembershipDto = new CourseMembershipDto
                {
                    User = new UserDto
                    {
                        FirstName = courseMembership.User.FirstName,
                        LastName = courseMembership.User.LastName,
                        Email = courseMembership.User.Email,
                    },
                    Course = new CourseDto
                    {
                        CourseCode = courseMembership.Course.CourseCode,
                        Name = courseMembership.Course.Name,
                        StartDate = courseMembership.Course.StartDate,
                        EndDate = courseMembership.Course.EndDate
                    },
                    EndrolledDate = courseMembership.EndrolledDate

                };
                return courseMembershipDto;
            }
            return NotFound();
        }
        /// <summary>
        /// Update CourseMembership by find the Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userEmail"></param>
        /// <param name="courseCode"></param>
        /// <param name="endRolledDate"></param>
        /// <returns></returns>
        // PUT: api/CourseMemberships/5
        [HttpPut]
        [Route("UpdateCourseMembership/{id}")]
        public async Task<IActionResult> PutCourseMembership(int id, string userEmail, string courseCode, DateTime endRolledDate)
        {
            //if (id != courseMembership.Id)
            //{
            //    return BadRequest();
            //}

            Course course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            User user = await _context.Users.FirstOrDefaultAsync(c => c.Email == userEmail);

            CourseMembership courseMembership = new CourseMembership()
            {
                Id = id,
                Course = course,
                User = user,
                EndrolledDate = endRolledDate
            };

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

            return StatusCode(200);
        }
        /// <summary>
        /// Delete CouseMembership by find the Id
        /// </summary>
        /// <param name="id">Identity for CourseMembership</param>
        /// <returns>return nothing</returns>
        // DELETE: api/CourseMemberships/5
        [HttpDelete]
        [Route("DeleteCourseMembership/{id}")]
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
