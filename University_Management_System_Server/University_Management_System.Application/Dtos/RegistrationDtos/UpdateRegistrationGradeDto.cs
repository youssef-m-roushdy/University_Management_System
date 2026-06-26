using University_Management_System.Domain.Enums;

namespace University_Management_System.Application.Dtos.RegistrationDtos
{
    public class UpdateRegistrationGradeDto
    {
        public int RegistrationId { get; set; }
        public Grades? Grade { get; set; }
    }
}