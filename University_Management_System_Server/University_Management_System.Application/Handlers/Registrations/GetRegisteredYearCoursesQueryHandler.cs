using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetRegisteredYearCoursesQueryHandler : IRequestHandler<GetRegisteredYearCoursesQuery, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRegisteredYearCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            GetRegisteredYearCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var studyYear = await _unitOfWork.StudyYears.GetByIdAsync(request.StudyYearId);
            if (studyYear == null)
                throw new NotFoundException($"Study Year with ID '{request.StudyYearId}' not found.");

            var (registrations, _) = await _unitOfWork.Registrations
                .GetByStudentAndStudyYearAsync(
                    request.StudentId,
                    request.StudyYearId,
                    null,
                    cancellationToken);

            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}