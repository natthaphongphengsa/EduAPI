using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Data.Models
{
    public class CourseMembership
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Course Course { get; set; }
        public DateTime? EndrolledDate { get; set; }
    }
}
