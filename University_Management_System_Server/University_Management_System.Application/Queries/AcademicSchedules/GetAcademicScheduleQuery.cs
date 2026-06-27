using MediatR;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;

namespace University_Management_System.Application.Queries.AcademicSchedules
{
    public class GetAcademicScheduleQuery : IRequest<AcademicScheduleDto>
    {
        public int Id { get; set; }
    }
}