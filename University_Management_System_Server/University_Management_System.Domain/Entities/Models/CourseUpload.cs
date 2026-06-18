using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities;
using University_Management_System.Domain.Entities.Identity;
using University_Management_System.Domain.Entities.Models;

namespace University_Management_System.Domain.Entities.Models
{
    public class CourseUpload : BaseEntities<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // e.g., "sheet", "sheet answer", "material", etc.
        public string FileId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public Collection<InstructorCourseUpload> InstructorCourseUploads { get; set; } = new Collection<InstructorCourseUpload>();
        public Collection<AssistantCourseUpload> AssistantCourseUploads { get; set; } = new Collection<AssistantCourseUpload>();
    }
}