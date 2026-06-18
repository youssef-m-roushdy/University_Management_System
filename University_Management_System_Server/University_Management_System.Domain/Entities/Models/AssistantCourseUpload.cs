using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace University_Management_System.Domain.Entities.Models
{
    public class AssistantCourseUpload : BaseEntities<int>
    {
        // this model of the many-to-many relationship between InstructorAssistant and CourseUpload
        //show the course uplaod of assistant in a specific semester and study year
        public string AssistantId { get; set; } = string.Empty;
        public Assistant Assistant { get; set; } = null!;

        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;

        public int StudyYearId { get; set; }
        public StudyYear StudyYear { get; set; } = null!;

        public Collection<CourseUpload> CourseUploads { get; set; } = new Collection<CourseUpload>();
    }
}