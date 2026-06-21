using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class UpdateStudyYearCommandHandler : IRequestHandler<UpdateStudyYearCommand, StudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateStudyYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudyYearDto> Handle(UpdateStudyYearCommand request, CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Id);
            if (studyYear == null)
                throw new NotFoundException($"Study year with ID '{request.Id}' not found");

            // Check if another study year with same range exists
            var exists = await _unitOfWork.StudyYears
                .StudyYearExistsAsync(request.StartYear, request.EndYear);

            if (exists && (studyYear.StartYear != request.StartYear || studyYear.EndYear != request.EndYear))
                throw new ValidationException(new List<string> { "Study year already exists" });

            // If setting as current, unset previous current
            if (request.IsCurrent && !studyYear.IsCurrent)
            {
                var current = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();
                if (current != null && current.Id != request.Id)
                {
                    current.IsCurrent = false;
                    await _unitOfWork.StudyYears.UpdateAsync(current);
                }
            }

            // ✅ Use AutoMapper to map Command to existing Entity
            _mapper.Map(request, studyYear);
            studyYear.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.StudyYears.UpdateAsync(studyYear);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudyYearDto>(studyYear);
        }
    }
}