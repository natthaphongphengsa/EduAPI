using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Dtos
{
    public class CourseMembershipDto
    {
        [Required]
        public UserDto User { get; set; }
        [Required]
        public CourseDto Course { get; set; }
        public DateTime? EndrolledDate { get; set; }
    }
}
