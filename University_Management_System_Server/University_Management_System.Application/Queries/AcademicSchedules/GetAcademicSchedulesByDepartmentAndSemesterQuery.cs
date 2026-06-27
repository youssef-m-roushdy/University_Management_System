using MediatR;
using University_Management_System.Application.Dtos.AcademicScheduleDtos;
using University_Management_System.Domain.Queries.AcademicScheduleQueries;

namespace University_Management_System.Application.Queries.AcademicSchedules
{
    public class GetAcademicSchedulesByDepartmentAndSemesterQuery : IRequest<(IEnumerable<AcademicScheduleDto> Data, int TotalCount)>
    {
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }
        public AcademicScheduleDepartmentSemesterQueries? Filter { get; set; }
    }
}