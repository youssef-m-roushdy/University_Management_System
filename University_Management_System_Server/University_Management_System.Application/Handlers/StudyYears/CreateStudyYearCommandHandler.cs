using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Domain.Entities.Models;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class CreateStudyYearCommandHandler : IRequestHandler<CreateStudyYearCommand, StudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateStudyYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudyYearDto> Handle(CreateStudyYearCommand request, CancellationToken cancellationToken)
        {
            // Check if study year already exists
            var exists = await _unitOfWork.StudyYears
                .StudyYearExistsAsync(request.StartYear, request.EndYear);

            if (exists)
                throw new ValidationException(new List<string> { "Study year already exists" });

            // If setting as current, unset previous current
            if (request.IsCurrent)
            {
                var current = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();
                if (current != null)
                {
                    current.IsCurrent = false;
                    await _unitOfWork.StudyYears.UpdateAsync(current);
                }
            }

            // ✅ Use AutoMapper to map Command to Entity
            var studyYear = _mapper.Map<StudyYear>(request);

            await _unitOfWork.StudyYears.AddAsync(studyYear);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudyYearDto>(studyYear);
        }
    }
}