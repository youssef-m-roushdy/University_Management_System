using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class UpdateStudentStudyYearCommandHandler : IRequestHandler<UpdateStudentStudyYearCommand, ApiResponse<StudentStudyYearDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentStudyYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<StudentStudyYearDto>> Handle(UpdateStudentStudyYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.StudentStudyYears.GetByIdAsync(request.Id);
            if (entity is null)
                return ApiResponse<StudentStudyYearDto>.ErrorResponse("Student study year record not found.");

            var dto = request.Dto;

            if (dto.Level.HasValue)
                entity.Level = dto.Level.Value;

            await _unitOfWork.StudentStudyYears.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // Re-fetch with includes
            var updated = await _unitOfWork.StudentStudyYears.GetByStudentAndStudyYearAsync(entity.StudentId, entity.StudyYearId);

            return ApiResponse<StudentStudyYearDto>.SuccessResponse(MapToDto(updated!));
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
