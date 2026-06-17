using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Domain.Contracts;
using MediatR;

namespace University_Management_System.Application.Handlers.StudyYears;

public class GetAllStudyYearsQueryHandler : IRequestHandler<GetAllStudyYearsQuery, IEnumerable<StudyYearDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllStudyYearsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<StudyYearDto>> Handle(GetAllStudyYearsQuery request, CancellationToken cancellationToken)
    {
        var studyYears = await _unitOfWork.StudyYears.GetAllAsync();
        return studyYears
            .OrderByDescending(sy => sy.StartYear)
            .Select(sy => new StudyYearDto
            {
                Id = sy.Id,
                StartYear = sy.StartYear,
                EndYear = sy.EndYear,
                IsCurrent = sy.IsCurrent
            });
    }
}
