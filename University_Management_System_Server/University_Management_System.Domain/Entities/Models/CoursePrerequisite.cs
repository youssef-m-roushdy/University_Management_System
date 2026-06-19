using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Models
{
    public class CoursePrerequisite: BaseEntities<int>
    {
        public int CourseId { get; set; } // The course that requires prerequisites
        public Course Course { get; set; } = null!;

        public int PrerequisiteCourseId { get; set; } // The course that is required
        public Course PrerequisiteCourse { get; set; } = null!;
    }
}