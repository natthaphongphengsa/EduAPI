using EduAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Dtos
{
    public class CourseMembershipDto
    {
        public UserDto User { get; set; }
        public CourseDto Course { get; set; }
        public DateTime? EndrolledDate { get; set; }
    }
}
