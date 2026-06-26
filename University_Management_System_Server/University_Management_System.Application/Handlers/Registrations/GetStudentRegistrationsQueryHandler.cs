using AutoMapper;
using MediatR;
using University_Management_System.Application.Dtos.RegistrationDtos;
using University_Management_System.Application.Queries.Registrations;
using University_Management_System.Domain.Contracts;
using University_Management_System.Shared.Exceptions;

namespace University_Management_System.Application.Handlers.Registrations
{
    public class GetStudentRegistrationsQueryHandler : IRequestHandler<GetStudentRegistrationsQuery, IEnumerable<RegistrationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStudentRegistrationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RegistrationDto>> Handle(
            GetStudentRegistrationsQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(request.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID '{request.StudentId}' not found.");

            var (registrations, _) = await _unitOfWork.Registrations
                .GetByStudentIdAsync(request.StudentId, null, cancellationToken);

            return _mapper.Map<IEnumerable<RegistrationDto>>(registrations);
        }
    }
}