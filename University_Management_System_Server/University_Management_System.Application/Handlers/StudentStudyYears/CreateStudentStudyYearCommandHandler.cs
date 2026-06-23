using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Application.Queries.StudentStudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;
using University_Management_System.Application.Commands.StudentStudyYears;
using AutoMapper;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class CreateStudentStudyYearCommandHandler : IRequestHandler<CreateStudentStudyYearCommand, StudentStudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateStudentStudyYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentStudyYearDto> Handle(CreateStudentStudyYearCommand request, CancellationToken cancellationToken)
        {
            // Check if student exists
            var student = await _unitOfWork.Students.GetByIdAsync(request.Dto.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.Dto.StudentId}' not found.");

            // Check if study year exists
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Dto.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study year with ID '{request.Dto.StudyYearId}' not found.");

            // Check if student is already enrolled in this study year
            var existingEnrollment = await _unitOfWork.StudentStudyYears
                .IsStudentEnrolledAsync(request.Dto.StudentId, request.Dto.StudyYearId);
            
            if (existingEnrollment)
                throw new ValidationException(new List<string> { 
                    $"Student is already enrolled in study year {studyYear.StartYear}-{studyYear.EndYear}" 
                });

            // Create enrollment
            var enrollment = new StudentStudyYear
            {
                StudentId = request.Dto.StudentId,
                StudyYearId = request.Dto.StudyYearId,
                Level = request.Dto.Level,
                IsActive = request.Dto.IsActive,
                EnrolledAt = DateTime.UtcNow
            };

            await _unitOfWork.StudentStudyYears.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            // Get full details
            var result = await _unitOfWork.StudentStudyYears
                .GetByStudentAndStudyYearAsync(request.Dto.StudentId, request.Dto.StudyYearId);

            return _mapper.Map<StudentStudyYearDto>(result);
        }
    }
}