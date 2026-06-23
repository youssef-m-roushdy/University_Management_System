using AutoMapper;
using MediatR;
using University_Management_System.Application.Commands.StudyYears;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class PatchStudyYearCommandHandler : IRequestHandler<PatchStudyYearCommand, StudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatchStudyYearCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudyYearDto> Handle(PatchStudyYearCommand request, CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Id);
            if (studyYear == null)
                throw new NotFoundException($"Study year with ID '{request.Id}' not found");

          
            var current = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();

            if (current != null && current.Id != request.Id)
            {
                throw new ConflictException("A current study year already exists");
            }

            studyYear.IsCurrent = request.Dto.IsCurrent;
            studyYear.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<StudyYearDto>(studyYear);
        }
    }
}