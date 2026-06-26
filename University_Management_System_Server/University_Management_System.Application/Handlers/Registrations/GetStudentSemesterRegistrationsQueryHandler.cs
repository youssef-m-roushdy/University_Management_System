using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetStudentSemesterRegistrationsQueryHandler : IRequestHandler<GetStudentSemesterRegistrationsQuery, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentSemesterRegistrationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            GetStudentSemesterRegistrationsQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.StudentId}' not found.");

            var semester = await _unitOfWork.Semesters.GetByIdAsync(request.SemesterId);
            if (semester == null)
                throw new NotFoundException($"Semester with ID '{request.SemesterId}' not found.");

            var registrations = await _unitOfWork.Registrations
                .GetStudentSemesterCoursesAsync(request.StudentId, 0, request.SemesterId);

            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}