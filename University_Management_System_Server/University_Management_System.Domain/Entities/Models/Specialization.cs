using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Identity;

namespace University_Management_System.Domain.Entities.Models
{
    public class Specialization : BaseEntities<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // specialization belongs to one department
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        // Navigation properties
        public ICollection<SpecializationCourse> SpecializationCourses { get; set; } = new List<SpecializationCourse>();
        public ICollection<User> Users { get; set; } = new List<User>(); // mean the specialization has multiple users and the user belongs to one specialization
    }
}