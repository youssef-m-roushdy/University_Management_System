using University_Management_System.Application.Dtos.UserDtos;

namespace University_Management_System.Application.Dtos.InstructorDtos
{
    public class CreateInstructorDto : CreateUserDto
    {
        public int DepartmentId { get; set; }
    }
}