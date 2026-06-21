using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class GetStudyYearQueryHandler : IRequestHandler<GetStudyYearQuery, StudyYearDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudyYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudyYearDto> Handle(GetStudyYearQuery request, CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.Id);
            if (studyYear == null)
                throw new NotFoundException($"Study year with ID '{request.Id}' not found");

            var dto = _mapper.Map<StudyYearDto>(studyYear);
            
            // Get counts
            dto.SemesterCount = await _unitOfWork.StudyYears.GetSemesterCountAsync(request.Id);
            dto.StudentCount = await _unitOfWork.StudyYears.GetStudentCountAsync(request.Id);
            dto.RegistrationCount = await _unitOfWork.StudyYears.GetRegistrationCountAsync(request.Id);

            return dto;
        }
    }
}