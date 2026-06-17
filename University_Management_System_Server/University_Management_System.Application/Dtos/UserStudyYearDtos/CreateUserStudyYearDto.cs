using System.ComponentModel.DataAnnotations;
using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.UserStudyYearDtos
{
    public class CreateUserStudyYearDto
    {
        public string UserId { get; set; } = string.Empty;
        public int StudyYearId { get; set; }
        public Levels Level { get; set; }
    }
}
