using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.SemesterGPADtos;
using University_Management_System.Application.Queries.SemesterGPAs;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.SemesterGPAs
{
    public class GetStudyYearSemesterGPAsQueryHandler : IRequestHandler<GetStudyYearSemesterGPAsQuery, (IEnumerable<SemesterGPADto> Data, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearSemesterGPAsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<SemesterGPADto> Data, int TotalCount)> Handle(
            GetStudyYearSemesterGPAsQuery request,
            CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.StudyYearId}' not found");

            var (semesterGPAs, totalCount) = await _unitOfWork.SemesterGPAs
                .GetByStudyYearIdPaginatedAsync(request.StudyYearId, request.Filter, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<SemesterGPADto>>(semesterGPAs);

            foreach (var dto in dtos)
            {
                var entity = semesterGPAs.First(s => s.Id == dto.Id);
                dto.StudentName = entity.Student?.User?.Name ?? string.Empty;
                dto.AcademicCode = entity.Student?.AcademicCode ?? string.Empty;
                dto.StudyYearRange = entity.StudyYear != null 
                    ? $"{entity.StudyYear.StartYear}-{entity.StudyYear.EndYear}" 
                    : string.Empty;
                dto.DepartmentName = entity.Student?.Department?.Name ?? string.Empty;
                dto.DepartmentCode = entity.Student?.Department?.Code ?? string.Empty;
            }

            return (dtos, totalCount);
        }
    }
}