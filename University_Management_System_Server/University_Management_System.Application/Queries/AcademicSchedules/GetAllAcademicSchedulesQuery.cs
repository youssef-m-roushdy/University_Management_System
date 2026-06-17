using University_Management_System.Application.Dtos.AcademicSheduleDtos;
using MediatR;

namespace University_Management_System.Application.Queries.AcademicSchedules
{
    public class GetAllAcademicSchedulesQuery : IRequest<List<AcademicSchedulesDto>>
    {
    }
}