using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Respones;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class CreateStudentStudyYearCommandHandler : IRequestHandler<CreateStudentStudyYearCommand, Response<StudentStudyYearDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentStudyYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<StudentStudyYearDto>> Handle(CreateStudentStudyYearCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Validate StudyYear exists
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(dto.StudyYearId);
            if (studyYear is null)
                return Response<StudentStudyYearDto>.ErrorResponse("Study year not found.");

            // Check if already enrolled in this study year
            var existing = await _unitOfWork.StudentStudyYears.GetByStudentAndStudyYearAsync(dto.StudentId, dto.StudyYearId);
            if (existing is not null)
                return Response<StudentStudyYearDto>.ErrorResponse("Student is already enrolled in this study year.");

            var entity = new StudentStudyYear
            {
                StudentId = dto.StudentId,
                StudyYearId = dto.StudyYearId,
                Level = dto.Level,
                EnrolledAt = DateTime.UtcNow
            };

            await _unitOfWork.StudentStudyYears.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // Re-fetch with includes
            var saved = await _unitOfWork.StudentStudyYears.GetByStudentAndStudyYearAsync(dto.StudentId, dto.StudyYearId);

            var resultDto = MapToDto(saved!);
            return Response<StudentStudyYearDto>.SuccessResponse(resultDto);
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
