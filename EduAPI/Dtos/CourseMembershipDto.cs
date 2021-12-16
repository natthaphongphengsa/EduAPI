using EduAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Dtos
{
    public class CourseMembershipDto
    {
        public User User { get; set; }
        public Course Course { get; set; }
        public DateTime EndrolledDate { get; set; }
    }
}
