using MediatR;
using University_Management_System.Application.Dtos.SemesterGPADtos;
using University_Management_System.Domain.Queries.SemesterGPAQueries;

namespace University_Management_System.Application.Queries.SemesterGPAs
{
    public class GetSemesterGPAsBySemesterQuery : IRequest<(IEnumerable<SemesterGPADto> Data, int TotalCount)>
    {
        public int SemesterId { get; set; }
        public SemesterGPAFilterInSemesterQueries Filter { get; set; } = new SemesterGPAFilterInSemesterQueries();
    }
}