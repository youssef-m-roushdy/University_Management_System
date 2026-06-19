using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetStudentStudyYearsQueryHandler : IRequestHandler<GetStudentStudyYearsQuery, ApiResponse<List<StudentStudyYearDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentStudyYearsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<StudentStudyYearDto>>> Handle(GetStudentStudyYearsQuery request, CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.StudentStudyYears.GetByStudentIdAsync(request.StudentId);

            var dtos = records.Select(MapToDto).ToList();

            return ApiResponse<List<StudentStudyYearDto>>.SuccessResponse(dtos);
        }

        private static StudentStudyYearDto MapToDto(StudentStudyYear entity)
        {
            return new StudentStudyYearDto
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                StudyYearId = entity.StudyYearId,
                StartYear = entity.StudyYear?.StartYear ?? 0,
                EndYear = entity.StudyYear?.EndYear ?? 0,
                Level = entity.Level,
                LevelName = entity.Level.ToString().Replace("_", " "),
                IsCurrent = entity.StudyYear?.IsCurrent ?? false,
                EnrolledAt = entity.EnrolledAt
            };
        }
    }
}
