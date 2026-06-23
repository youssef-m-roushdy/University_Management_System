using University_Management_System.Application.Commands.StudentStudyYears;
using University_Management_System.Application.Dtos.StudentStudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using MediatR;
using University_Management_System.Shared.Responses;
using AutoMapper;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudentStudyYears
{
    public class UpdateStudentStudyYearCommandHandler : IRequestHandler<UpdateStudentStudyYearCommand, StudentStudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateStudentStudyYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudentStudyYearDto> Handle(UpdateStudentStudyYearCommand request, CancellationToken cancellationToken)
        {
            var enrollment = await _unitOfWork.StudentStudyYears.GetByIdAsync(request.Id);
            if (enrollment == null)
                throw new NotFoundException($"Enrollment with ID '{request.Id}' not found.");

            // Update fields
            if (request.Dto.Level.HasValue)
                enrollment.Level = request.Dto.Level.Value;

            if (request.Dto.IsActive.HasValue)
                enrollment.IsActive = request.Dto.IsActive.Value;
                
            enrollment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.StudentStudyYears.UpdateAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            // Get full details
            var result = await _unitOfWork.StudentStudyYears
                .GetByStudentAndStudyYearAsync(enrollment.StudentId, enrollment.StudyYearId);

            return _mapper.Map<StudentStudyYearDto>(result);
        }
    }
}