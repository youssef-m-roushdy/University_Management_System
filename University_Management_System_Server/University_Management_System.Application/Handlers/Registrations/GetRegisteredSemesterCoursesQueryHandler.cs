using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetRegisteredSemesterCoursesQueryHandler : IRequestHandler<GetRegisteredSemesterCoursesQuery, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRegisteredSemesterCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            GetRegisteredSemesterCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.StudyYearId}' not found.");

            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.SemesterId);
            if (semester == null)
                throw new NotFoundException($"Semester with ID '{request.SemesterId}' not found.");

            var isSemesterInStudyYear = await _unitOfWork.Semesters
                .IsSemesterBelongsToStudyYearAsync(request.SemesterId, request.StudyYearId);
            
            if (!isSemesterInStudyYear)
                throw new ValidationException(new List<string> {
                    $"Semester with ID '{request.SemesterId}' does not belong to Study Year '{request.StudyYearId}'."
                });

            var (registrations, _) = await _unitOfWork.Registrations
                .GetByStudentAndStudyYearAndSemesterAsync(
                    request.StudentId,
                    request.StudyYearId,
                    request.SemesterId,
                    null,
                    cancellationToken);

            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}