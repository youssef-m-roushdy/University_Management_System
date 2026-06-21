using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.StudyYearDtos;
using University_Management_System.Application.Queries.StudyYears;
using University_Management_System.Domain.Contracts;

namespace University_Management_System.Application.Handlers.StudyYears
{
    public class GetCurrentStudyYearQueryHandler : IRequestHandler<GetCurrentStudyYearQuery, StudyYearDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCurrentStudyYearQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StudyYearDto?> Handle(GetCurrentStudyYearQuery request, CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetCurrentStudyYearAsync();
            
            if (studyYear == null)
                return null;

            var dto = _mapper.Map<StudyYearDto>(studyYear);
            
            // Get counts
            dto.SemesterCount = await _unitOfWork.StudyYears.GetSemesterCountAsync(studyYear.Id);
            dto.StudentCount = await _unitOfWork.StudyYears.GetStudentCountAsync(studyYear.Id);
            dto.RegistrationCount = await _unitOfWork.StudyYears.GetRegistrationCountAsync(studyYear.Id);

            return dto;
        }
    }
}