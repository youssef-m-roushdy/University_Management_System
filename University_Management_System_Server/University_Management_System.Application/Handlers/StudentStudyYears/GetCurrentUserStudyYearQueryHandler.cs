using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class GetCurrentStudentStudyYearQueryHandler : IRequestHandler<GetCurrentStudentStudyYearQuery, ApiResponse<StudentStudyYearDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentStudentStudyYearQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<StudentStudyYearDto>> Handle(GetCurrentStudentStudyYearQuery request, CancellationToken cancellationToken)
        {
            var current = await _unitOfWork.StudentStudyYears.GetCurrentByStudentIdAsync(request.StudentId);
            if (current is null)
                return ApiResponse<StudentStudyYearDto>.ErrorResponse("No current study year found for this Student.");

            return ApiResponse<StudentStudyYearDto>.SuccessResponse(MapToDto(current));
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
