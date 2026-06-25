using MediatR;
using University_Management_System.Application.Dtos.InstructorDtos;

namespace University_Management_System.Application.Queries.Instructors
{
    public class GetInstructorQuery : IRequest<InstructorDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}