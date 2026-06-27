using MediatR;
using University_Management_System.Application.Dtos.SemesterGPADtos;

namespace University_Management_System.Application.Queries.SemesterGPAs
{
    public class GetStudentSemesterGPAQuery : IRequest<SemesterGPADto>
    {
        public int SemesterGPAId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}